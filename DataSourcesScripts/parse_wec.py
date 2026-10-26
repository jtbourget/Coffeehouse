import PyPDF2
import re

def get_ocd_id(office_name):
    office_name = office_name.strip().upper()
    state_ocd = "ocd-division/country:us/state:wi"
    
    if "CONGRESS DISTRICT" in office_name:
        m = re.search(r'DISTRICT\s+(\d+)', office_name)
        if m: return f"{state_ocd}/cd:{m.group(1)}"
    elif "STATE SENATOR DISTRICT" in office_name:
        m = re.search(r'DISTRICT\s+(\d+)', office_name)
        if m: return f"{state_ocd}/sldu:{m.group(1)}"
    elif "REPRESENTATIVE TO THE ASSEMBLY DISTRICT" in office_name:
        m = re.search(r'DISTRICT\s+(\d+)', office_name)
        if m: return f"{state_ocd}/sldl:{m.group(1)}"
    else:
        return state_ocd

def parse_pdf(filepath):
    reader = PyPDF2.PdfReader(filepath)
    text = ""
    for page in reader.pages:
        text += page.extract_text() + "\n"
        
    contests = {}
    lines = [l.strip() for l in text.split('\n') if l.strip()]
    i = 0
    current_office = None
    
    while i < len(lines):
        if lines[i] == 'Office :':
            current_office = lines[i+1]
            contests[current_office] = []
            i += 2
            continue
            
        if current_office and lines[i].isdigit() and int(lines[i]) < 10:
            # It's a candidate
            i += 1
            if i < len(lines) and lines[i].isdigit() and len(lines[i]) > 5:
                i += 1
            
            if i < len(lines):
                name = lines[i]
                i += 1
                party = lines[i] if i < len(lines) else "Unknown"
                
                # Check for "Serving People Not Parties"
                if party == "Serving People":
                    party = "Serving People Not Parties"
                    i += 1 # skip Not Parties
                
                contests[current_office].append({
                    "name": name,
                    "party": party
                })
        i += 1
        
    return contests

def generate_cs(contests):
    cs_code = """using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Coffeehouse.Api.Data
{
    public static class WiCandidateSeeder
    {
        public static void SeedWisconsinElections(AppDbContext context)
        {
            var electionName = "2026 Wisconsin General Election";
            var election = context.Elections.Include(e => e.Contests).ThenInclude(c => c.Candidates)
                .FirstOrDefault(e => e.Name == electionName);
            
            bool isNew = false;
            if (election == null)
            {
                election = new Election { Name = electionName, ElectionDate = new System.DateTime(2026, 11, 3), Contests = new List<Contest>() };
                isNew = true;
            }

            var incomingContests = new List<Contest>
            {
"""
    for office, cands in contests.items():
        if not cands: continue
        ocd_id = get_ocd_id(office)
        cs_code += f'                new Contest\n'
        cs_code += f'                {{\n'
        cs_code += f'                    OfficeName = "{office}",\n'
        cs_code += f'                    OcdId = "{ocd_id}",\n'
        cs_code += f'                    Candidates = new List<Candidate>\n'
        cs_code += f'                    {{\n'
        for idx, c in enumerate(cands):
            pic = f"https://picsum.photos/200?random={hash(c['name'] + office) % 1000}"
            website = ""
            
            if "Tom Tiffany" in c["name"]:
                pic = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d6/Tom_Tiffany.jpg/960px-Tom_Tiffany.jpg"
                website = "https://tomtiffany.com/"
                
            name = c["name"].replace('"', '\\"')
            cs_code += f'                        new Candidate {{ Name = "{name}", Party = "{c["party"]}", PhotoUrl = "{pic}", CampaignWebsite = "{website}" }}'
            if idx < len(cands) - 1:
                cs_code += ","
            cs_code += "\n"
        cs_code += f'                    }}\n'
        cs_code += f'                }},\n'
        
    cs_code = cs_code.rstrip(",\n") + "\n"
    
    cs_code += """            };

            foreach (var incContest in incomingContests)
            {
                var existingContest = election.Contests.FirstOrDefault(c => c.OfficeName == incContest.OfficeName);
                if (existingContest == null)
                {
                    election.Contests.Add(incContest);
                }
                else
                {
                    foreach (var incCand in incContest.Candidates)
                    {
                        var existingCand = existingContest.Candidates.FirstOrDefault(c => c.Name == incCand.Name);
                        if (existingCand == null)
                        {
                            existingContest.Candidates.Add(incCand);
                        }
                        else
                        {
                            // Upsert logic: Update the picture, party, and website if changed
                            existingCand.PhotoUrl = incCand.PhotoUrl;
                            existingCand.Party = incCand.Party;
                            existingCand.CampaignWebsite = incCand.CampaignWebsite;
                        }
                    }
                }
            }

            if (isNew) context.Elections.Add(election);
            context.SaveChanges();
        }
    }
}
"""
    return cs_code

if __name__ == "__main__":
    contests = parse_pdf("Candidates on Ballot By Election_November 3 2026 General Election_0.pdf")
    cs = generate_cs(contests)
    with open("Coffeehouse.Api/Data/WiCandidateSeeder.cs", "w", encoding="utf-8") as f:
        f.write(cs)
    print("Done")
