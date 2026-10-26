# Coffeehouse Voting API & .NET MAUI Client

A full-stack, secure, end-to-end voting application following the teaching architecture of the semester. The application provides civic data and manages **Candidate Profiles** (`/api/candidateprofiles` and `/api/candidates`).

The solution features:
- **Switchable Data Source:** Single repository interface (`ICandidateProfileRepository`) backed interchangeably by a thread-safe local JSON store (`JsonStoreState`) or an Entity Framework Core SQL Server database.
- **SQL Server in Docker:** Containerized SQL Server with health checks defined in `compose.yaml`. Sensitive SA credentials live in `.env` (git-ignored) and are auto-generated on first run.
- **JWT Authentication & Authorization:** Secure `POST /api/auth/login` endpoint issuing JSON Web Tokens. Write operations (`POST`, `PUT`, `DELETE`) are guarded by the `CandidateProfileWrite` policy, while `GET` endpoints remain public.
- **Structured Logging:** Clear console startup lines indicating the active data source provider and authentication status, accompanied by audit log entries for every create, update, and delete operation.
- **.NET MAUI MVVM Client:** Cross-platform native client (Windows and Android emulator) with Shell navigation, `SecureStorage` token persistence with in-memory fallback, delegating HTTP handler for bearer authorization, and real-time form validation error mapping.
- **Verified Postman Collection:** Automated test suite runnable all-green with authentication disabled and all-green with authentication enabled.

---

## 1. Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/) (running)
- Visual Studio 2022 with the *.NET Multi-platform App UI* workload (for MAUI debugging)
- Trusted development certificate (run once):
  ```powershell
  dotnet dev-certs https --trust
  ```

---

## 2. Running SQL Server in Docker

To spin up the containerized SQL Server manually:
```powershell
# In the repository root:
docker compose up -d sqlserver
```

Verify that the container is healthy:
```powershell
docker compose ps
```

> [!NOTE]
> On the first API run in `Development` mode, `DevelopmentBootstrap` will automatically create `.env` with a strong SA password and run `docker compose up -d --wait sqlserver` if Docker Desktop is open.

---

## 3. Running the API

From the repository root:

```powershell
# Run with HTTPS profile (listens on https://localhost:7049 and http://localhost:5032)
dotnet run --project Coffeehouse.Api --launch-profile https
```

Or with the HTTP profile:
```powershell
dotnet run --project Coffeehouse.Api --launch-profile http
```

### Startup Log Verification
When the API boots, it writes the active configuration state to the console:
```text
info: Coffeehouse.Api.Program[0]
      Active data source provider: SqlServer
info: Coffeehouse.Api.Program[0]
      Authentication enabled: True
```

### Local Setup Commands
The API includes utility commands for classroom setup:
- `dotnet run --project Coffeehouse.Api -- --setup-sql`: Creates the database, executes migrations, creates a restricted `CoffeehouseApp` SQL login, and stores the connection string in User Secrets.
- `dotnet run --project Coffeehouse.Api -- --setup-auth`: Interactively prompts for a demo username/password and writes the hashed credentials and signing key to User Secrets.
- `dotnet run --project Coffeehouse.Api -- --reset-data`: Resets the active provider (`Json` or `SqlServer`) back to the initial seed profiles.

---

## 4. Configuration Switches

In `Coffeehouse.Api/appsettings.Development.json`:

```json
{
  "DataSource": {
    "Provider": "SqlServer",        // Switch between "SqlServer" and "Json"
    "JsonFilePath": "Data/CandidateProfiles.json",
    "AutoStartDocker": true,
    "MigrateOnStartup": true
  },
  "Authentication": {
    "Enabled": true,               // Switch between true and false
    "AutoGenerateSigningKey": true
  }
}
```

### Seeded Demo Accounts
| Username | Password | Notes |
|---|---|---|
| `student` | `Student123!` | Default classroom demo account |
| `instructor` | `Instructor123!` | Alternate demo account |

---

## 5. Testing with Postman

Import both files located in the `postman/` folder into Postman:
1. `postman/CandidateProfiles_API.postman_collection.json`
2. `postman/Coffeehouse.postman_environment.json`

### Running the Collection
1. **With Authentication OFF:**
   - In `appsettings.Development.json`, set `"Authentication": { "Enabled": false }` and restart the API.
   - Run the collection in **Postman Collection Runner**.
   - **Result:** All tests pass green.
2. **With Authentication ON:**
   - In `appsettings.Development.json`, set `"Authentication": { "Enabled": true }` and restart the API.
   - Run the collection in **Postman Collection Runner**.
   - **Result:** `0. POST Login` acquires the JWT bearer token, all write operations pass with the token, and all tests pass green.

---

## 6. Running the .NET MAUI Client

### On Windows
```powershell
dotnet build Coffeehouse/Coffeehouse.csproj -f net10.0-windows10.0.19041.0 -t:Run
```

### On Android Emulator
Ensure an Android emulator (e.g., Pixel 7) is running, then execute:
```powershell
dotnet build Coffeehouse/Coffeehouse.csproj -f net10.0-android -t:Run
```

### Client Walkthrough & Behavior
1. **Browse Without Login:** Open the app and navigate to the **Candidates** tab. All candidate profiles are listed via anonymous `GET /api/candidateprofiles`.
2. **Protected Writes / Redirect to Login:** Tap **+ Add** or **Delete** while logged out. Attempting to write prompts a redirect to the **Sign In** screen with the banner: *"Please log in to make changes."*
3. **Log In:** The login screen is prefilled in `#if DEBUG` with `student` / `Student123!`. Tap **Log In** to authenticate. On success, the token is saved in `SecureStorage` and you return to your task. The header button toggles to **Log out**.
4. **Create a Record:** Fill in candidate name, district (OCD-ID), bio, and website URL, then tap **Save Profile**. The new candidate profile is created through the API and stored in SQL Server / JSON.
5. **Validation Errors:** Clear the name or district or enter invalid input and tap **Save Profile**. The form displays field-level validation errors (e.g., *"Candidate name is required."*).
6. **Edit and Delete:** Tap the created candidate, tap **Edit Profile**, modify details, and save. To delete, tap **Delete Profile**, confirm the alert, and verify the record is removed from the list.

---

## 7. Video Demonstration Script (8 Minutes or Less)

Follow this sequence for your project demo recording:

1. **Postman Runner (Auth OFF):** Show `appsettings.Development.json` with `"Authentication:Enabled": false`. Start API. Run Postman Runner. Show all 13 tests green.
2. **Postman Runner (Auth ON):** Change `"Authentication:Enabled": true`. Restart API. Run Postman Runner. Show all 13 tests green.
3. **Data Source Switch & Docker Container:** Show `docker compose ps` with `coffeehouse-sqlserver` running and healthy. Point out `"DataSource:Provider": "SqlServer"`. Restart API and show that the same endpoints return data.
4. **Startup Log Lines:** Highlight the two startup lines in the terminal:
   - `Active data source provider: SqlServer`
   - `Authentication enabled: True`
5. **MAUI Anonymous Browsing & Write Interception:**
   - Launch MAUI app. Browse the candidate list without logging in.
   - Tap **+ Add** and try to save an empty or new record. Show the app navigating to the login page with the notice *"Please log in to make changes."*
6. **Login & Full CRUD:**
   - Tap **Log In** (credentials prefilled).
   - Create a new candidate profile (e.g. *Alex Voter*).
   - Demonstrate a validation error first (e.g. clear the name or enter an invalid district to show the red error message). Correct it and save.
   - Show the new candidate in the list.
   - Open detail, tap **Edit Profile**, change the bio or website, and save.
   - Tap **Delete Profile**, confirm deletion, and show the profile removed.
7. **30-Second Bug Story:**
   - *"One subtle bug I encountered during development was with delegating HTTP handlers on the Android emulator. In Android, localhost refers to the device itself rather than the host PC, requiring `10.0.2.2`. Because the ASP.NET Core dev certificate is issued to `CN=localhost`, the emulator initially rejected the SSL handshake. I resolved this by introducing a `#if DEBUG` `DevCertificateHandler` with a custom certificate callback that trusts `CN=localhost` specifically on Android in Debug builds, while preserving standard SSL validation in Release."*

---

## 8. Security & Git Hygiene

- No database passwords, JWT signing keys, or demo secrets are committed to Git.
- `.env`, `.env.*`, and `*.lock` are listed in `.gitignore`.
- Secrets are generated into `.NET User Secrets` using `UserSecretsId: d5fec6e5-142e-4b3d-b0b3-e833d11ce073`.
