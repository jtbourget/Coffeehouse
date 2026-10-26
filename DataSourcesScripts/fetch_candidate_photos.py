"""
fetch_candidate_photos.py
=========================
Extracts candidate names from the Wisconsin Elections Commission PDF in the
DataSources folder, fetches their official campaign profile pictures from
Ballotpedia (with Wikipedia fallback), updates the PhotoUrl in the Coffeehouse
database, and synchronizes WiCandidateSeeder.cs.

Usage:
    python DataSourcesScripts/fetch_candidate_photos.py [OPTIONS]

Options:
    --dry-run       Preview changes without writing to database or seeder file.
    --limit N       Process at most N candidates (useful for testing).
    --force         Re-fetch photos even if already found in cache.
"""

import argparse
import glob
import json
import os
import re
import subprocess
import sys
import time
import PyPDF2
import pyodbc
import requests
from bs4 import BeautifulSoup

# ---------------------------------------------------------------------------
# Configuration & Paths
# ---------------------------------------------------------------------------
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
PROJECT_ROOT = os.path.dirname(SCRIPT_DIR)
DATASOURCES_DIR = os.path.join(PROJECT_ROOT, "DataSources")
SEEDER_PATH = os.path.join(PROJECT_ROOT, "Coffeehouse.Api", "Data", "WiCandidateSeeder.cs")
CACHE_FILE = os.path.join(SCRIPT_DIR, "candidate_photos_cache.json")

DB_CONN_STR = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=(localdb)\\MSSQLLocalDB;"
    "Database=CoffeehouseDb;"
    "Trusted_Connection=Yes;"
)

USER_AGENT = (
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
    "AppleWebKit/537.36 (KHTML, like Gecko) "
    "Chrome/120.0.0.0 Safari/537.36"
)

# Patterns indicating placeholder / non-portrait images on Ballotpedia
PLACEHOLDER_PATTERNS = [
    "placeholder",
    "silhouette",
    "bp-initials",
    "submitphoto",
    "noimage",
    "no_image",
    "disambig",
    "question_mark",
    "ballotpedia_logo",
    "seal_of",
    "state_seal",
    "wisconsin_seal",
    "coat_of_arms",
    "flag_of",
]

# ---------------------------------------------------------------------------
# PDF Parsing
# ---------------------------------------------------------------------------

def find_datasources_pdf() -> str:
    """Find the candidates PDF file in the DataSources folder."""
    pdf_files = glob.glob(os.path.join(DATASOURCES_DIR, "*.pdf"))
    if not pdf_files:
        raise FileNotFoundError(f"No PDF files found in {DATASOURCES_DIR}")
    return pdf_files[0]


def extract_candidates_from_pdf(filepath: str) -> list[dict[str, str]]:
    """
    Parse candidates from the Wisconsin Elections Commission ballot PDF.
    Returns a list of dicts: [{'name': ..., 'party': ..., 'office': ...}, ...]
    """
    reader = PyPDF2.PdfReader(filepath)
    text = ""
    for page in reader.pages:
        text += page.extract_text() + "\n"

    candidates = []
    seen = set()
    lines = [l.strip() for l in text.split("\n") if l.strip()]
    i = 0
    current_office = None

    while i < len(lines):
        line = lines[i]
        if line == "Office :":
            current_office = lines[i + 1]
            i += 2
            continue

        if current_office and line.isdigit() and int(line) < 10:
            i += 1
            # Skip committee ID if present (6-8 digit number)
            if i < len(lines) and lines[i].isdigit() and len(lines[i]) > 5:
                i += 1

            if i < len(lines):
                name = lines[i]
                i += 1
                party = lines[i] if i < len(lines) else "Unknown"

                # Normalize party names
                if party == "Serving People":
                    party = "Serving People Not Parties"
                    i += 1
                elif party == "Wisconsin":
                    party = "Wisconsin Green"
                    if i + 1 < len(lines) and lines[i + 1] == "Green":
                        i += 1

                if name not in seen:
                    seen.add(name)
                    candidates.append({
                        "name": name,
                        "party": party,
                        "office": current_office,
                    })
        i += 1

    return candidates


# ---------------------------------------------------------------------------
# Scraping via curl (bypasses CloudFront TLS/WAF bot challenges)
# ---------------------------------------------------------------------------

def is_placeholder(src: str) -> bool:
    """Check if the image URL is a placeholder or default icon."""
    if not src:
        return True
    return any(p in src.lower() for p in PLACEHOLDER_PATTERNS)


def is_alt_placeholder(alt: str) -> bool:
    """Check if alt text indicates a placeholder (empty string is NOT a placeholder)."""
    if not alt:
        return False
    return any(p in alt.lower() for p in PLACEHOLDER_PATTERNS)


def curl_html(url: str) -> str:
    """Fetch HTML using curl.exe with browser headers to avoid CloudFront WAF blocks."""
    cmd = [
        "curl.exe", "-s", "-L", "--max-time", "15",
        "-H", f"User-Agent: {USER_AGENT}",
        "-H", "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
        "-H", "Accept-Language: en-US,en;q=0.5",
        url
    ]
    try:
        res = subprocess.run(cmd, capture_output=True, text=True, encoding="utf-8", errors="ignore")
        return res.stdout
    except Exception as exc:
        print(f"    [curl error] {exc}", flush=True)
        return ""


def extract_photo_from_html(html: str, candidate_name: str) -> str | None:
    """
    Extract candidate photo from Ballotpedia HTML, strictly ensuring the image
    belongs to the candidate and upgrading thumbnail size to 200x300.
    """
    if not html or len(html) < 300:
        return None
    soup = BeautifulSoup(html, "html.parser")

    clean_name = candidate_name.split(" / ")[0].strip()
    parts = clean_name.split()
    last_name = parts[-1].lower() if parts else ""
    first_name = parts[0].lower() if len(parts) > 1 else ""

    # 1. Div infobox (standard candidate page headshot)
    infobox = soup.find("div", class_="infobox")
    if infobox:
        img = infobox.find("img")
        if img:
            src = img.get("src", "")
            alt = img.get("alt", "") or ""
            if not is_placeholder(src) and not is_alt_placeholder(alt):
                # Upgrade thumbnail dimensions to high resolution
                return re.sub(r"/thumbs/\d+/\d+/", "/thumbs/200/300/", src)

    # 2. Table infobox (alternative candidate page headshot layout)
    table = soup.find("table", class_="infobox")
    if table:
        img = table.find("img")
        if img:
            src = img.get("src", "")
            alt = img.get("alt", "") or ""
            if not is_placeholder(src) and not is_alt_placeholder(alt):
                return re.sub(r"/thumbs/\d+/\d+/", "/thumbs/200/300/", src)

    # 3. Match candidate headshot by alt text (e.g. "Image of First Last")
    for img in soup.find_all("img"):
        alt = (img.get("alt") or "").lower()
        src = img.get("src") or ""
        if is_placeholder(src) or is_alt_placeholder(alt):
            continue
        if "image of" in alt and last_name in alt and first_name in alt:
            return re.sub(r"/thumbs/\d+/\d+/", "/thumbs/200/300/", src)

    # 4. Match candidate image by src filename
    for img in soup.find_all("img"):
        src = img.get("src") or ""
        if is_placeholder(src):
            continue
        src_lower = src.lower()
        if last_name in src_lower and first_name in src_lower and ("ballotpedia" in src_lower or "s3" in src_lower):
            return re.sub(r"/thumbs/\d+/\d+/", "/thumbs/200/300/", src)

    return None


def find_wisconsin_disambig_link(html: str) -> str | None:
    """If page is a disambiguation page, locate Wisconsin candidate link."""
    if not html:
        return None
    if "disambig" not in html.lower():
        return None

    soup = BeautifulSoup(html, "html.parser")
    for a in soup.find_all("a", href=True):
        href = a["href"]
        text = a.get_text().lower()
        if "wisconsin" in href.lower() or "wisconsin" in text:
            if not href.startswith("http"):
                href = f"https://ballotpedia.org{href}"
            return href
    return None


def query_wikipedia(name: str) -> str | None:
    """Fallback: lookup candidate portrait on Wikipedia via MediaWiki API."""
    url = "https://en.wikipedia.org/w/api.php"
    params = {
        "action": "query",
        "titles": name,
        "prop": "pageimages",
        "pithumbsize": 400,
        "format": "json"
    }
    try:
        r = requests.get(url, params=params, headers={"User-Agent": "CoffeehouseCivicApp/1.0 (info@coffeehouse.app)"}, timeout=6)
        if r.status_code == 200:
            pages = r.json().get("query", {}).get("pages", {})
            for pid, pdata in pages.items():
                if pid != "-1" and "thumbnail" in pdata:
                    return pdata["thumbnail"]["source"]
    except Exception:
        pass
    return None


def fetch_official_photo(candidate_name: str) -> str | None:
    """
    Search Ballotpedia and Wikipedia for an official candidate headshot.
    Tries multiple name variations, disambiguation resolution, and state qualifiers.
    """
    ticket_names = [n.strip() for n in candidate_name.split(" / ")]
    primary_name = ticket_names[0]

    variations = [primary_name]

    # Strip suffixes like ", Sr.", ", Jr.", " III"
    no_suffix = re.sub(r",?\s+(Sr\.?|Jr\.?|III|IV)$", "", primary_name, flags=re.IGNORECASE)
    if no_suffix != primary_name:
        variations.append(no_suffix)

    # Strip middle initials ("First M. Last" -> "First Last")
    no_initial = " ".join([p for p in primary_name.split() if not (len(p) <= 2 and p.endswith("."))])
    if no_initial not in variations:
        variations.append(no_initial)

    # Try each variation on Ballotpedia
    for var in variations:
        slug = var.replace(" ", "_")
        html = curl_html(f"https://ballotpedia.org/{slug}")

        # Check disambiguation
        disambig_url = find_wisconsin_disambig_link(html)
        if disambig_url:
            sub_html = curl_html(disambig_url)
            pic = extract_photo_from_html(sub_html, var)
            if pic:
                return pic

        pic = extract_photo_from_html(html, var)
        if pic:
            return pic

        # Try Wisconsin-specific page directly
        wi_html = curl_html(f"https://ballotpedia.org/{slug}_(Wisconsin)")
        pic = extract_photo_from_html(wi_html, var)
        if pic:
            return pic

    # Fallback to Wikipedia
    for var in variations:
        pic = query_wikipedia(var)
        if pic:
            return pic

    return None


# ---------------------------------------------------------------------------
# Cache Management
# ---------------------------------------------------------------------------

def load_cache() -> dict[str, str | None]:
    if os.path.exists(CACHE_FILE):
        try:
            with open(CACHE_FILE, "r", encoding="utf-8") as f:
                return json.load(f)
        except Exception:
            return {}
    return {}


def save_cache(cache: dict[str, str | None]) -> None:
    with open(CACHE_FILE, "w", encoding="utf-8") as f:
        json.dump(cache, f, indent=2)


# ---------------------------------------------------------------------------
# Database & Seeder Sync
# ---------------------------------------------------------------------------

def update_database(photo_map: dict[str, str]) -> int:
    """Update Candidates table in SQL Server with the official photo URLs."""
    if not photo_map:
        return 0

    conn = pyodbc.connect(DB_CONN_STR)
    cursor = conn.cursor()
    updated_count = 0

    for name, photo_url in photo_map.items():
        cursor.execute(
            "UPDATE Candidates SET PhotoUrl = ? WHERE Name = ?",
            photo_url, name
        )
        updated_count += cursor.rowcount

    conn.commit()
    conn.close()
    return updated_count


def update_seeder_code(photo_map: dict[str, str]) -> int:
    """Update WiCandidateSeeder.cs with the official photo URLs."""
    if not os.path.exists(SEEDER_PATH) or not photo_map:
        return 0

    with open(SEEDER_PATH, "r", encoding="utf-8") as f:
        content = f.read()

    changed = 0
    for name, photo_url in photo_map.items():
        escaped_name = re.escape(name)
        pattern = (
            rf'(Name\s*=\s*"{escaped_name}"\s*,\s*Party\s*=\s*"[^"]*"\s*,\s*'
            rf'PhotoUrl\s*=\s*")[^"]*(")'
        )
        new_content = re.sub(pattern, rf"\g<1>{photo_url}\2", content)
        if new_content != content:
            content = new_content
            changed += 1

    if changed > 0:
        with open(SEEDER_PATH, "w", encoding="utf-8") as f:
            f.write(content)

    return changed


# ---------------------------------------------------------------------------
# Main Execution
# ---------------------------------------------------------------------------

def main():
    parser = argparse.ArgumentParser(description="Fetch official candidate profile pictures and update database.")
    parser.add_argument("--dry-run", action="store_true", help="Preview results without updating database or code.")
    parser.add_argument("--limit", type=int, default=None, help="Process at most N candidates.")
    parser.add_argument("--force", action="store_true", help="Ignore cache and re-fetch all.")
    args = parser.parse_args()

    print("=" * 70)
    print(" Coffeehouse - Candidate Campaign Profile Picture Fetcher")
    print("=" * 70)
    if args.dry_run:
        print("[!] DRY RUN MODE - No changes will be saved to DB or seeder file.\n")

    # 1. Locate and parse candidate data from DataSources folder
    pdf_path = find_datasources_pdf()
    print(f"Reading candidates from: {os.path.basename(pdf_path)}")
    candidates = extract_candidates_from_pdf(pdf_path)
    total_candidates = len(candidates)
    print(f"Found {total_candidates} unique candidates in ballot document.\n")

    if args.limit:
        candidates = candidates[:args.limit]
        print(f"Limiting to first {args.limit} candidates.\n")

    # 2. Load cache
    cache = {} if args.force else load_cache()
    if cache:
        print(f"Loaded {len(cache)} cached candidate photo lookups.\n")

    # 3. Process candidates
    photo_map: dict[str, str] = {}
    found_count = 0
    from_cache_count = 0
    not_found_count = 0

    print("Fetching candidate profile pictures:")
    print("-" * 70)

    for idx, cand in enumerate(candidates, 1):
        name = cand["name"]
        office = cand["office"]

        # Check cache (only accept cache entries that have a valid photo)
        if name in cache and cache[name] and not args.force:
            photo = cache[name]
            photo_map[name] = photo
            found_count += 1
            from_cache_count += 1
            print(f"[{idx}/{len(candidates)}] (CACHE) {name} ({office}): {photo[:65]}...", flush=True)
            continue

        # Fetch live
        print(f"[{idx}/{len(candidates)}] Fetching: {name}...", end=" ", flush=True)
        photo = fetch_official_photo(name)
        cache[name] = photo
        save_cache(cache)

        if photo:
            photo_map[name] = photo
            found_count += 1
            print(f"FOUND -> {photo[:65]}...", flush=True)
        else:
            not_found_count += 1
            print("NOT FOUND", flush=True)

        time.sleep(0.2)  # Gentle delay between requests

    print("-" * 70)
    print(f"Scrape completed:")
    print(f"  Total processed:      {len(candidates)}")
    print(f"  Photos found:         {found_count} ({from_cache_count} from cache, {found_count - from_cache_count} newly retrieved)")
    print(f"  No photo found:       {not_found_count}")

    # 4. Update Database
    if not args.dry_run and photo_map:
        print("\nUpdating database...")
        db_updates = update_database(photo_map)
        print(f"Successfully updated {db_updates} candidate record(s) in CoffeehouseDb database.")

        print("Updating WiCandidateSeeder.cs...")
        seeder_updates = update_seeder_code(photo_map)
        print(f"Successfully updated {seeder_updates} candidate photo URL(s) in WiCandidateSeeder.cs.")
    elif args.dry_run:
        print("\n[DRY RUN] Skipping database and seeder file updates.")

    print("\nDone!")


if __name__ == "__main__":
    main()
