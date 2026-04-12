# Nexus — ASP.NET Core MVC Operations Dashboard

A redesigned dashboard with **SQLite persistence**, **cookie-based login**, and an **OpsPulse-inspired dark SaaS UI**.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Run the app
```bash
dotnet restore
dotnet run
```
Open `http://localhost:5000` in your browser.

---

## 🔐 Default Login Credentials

| Role   | Email              | Password     |
|--------|--------------------|--------------|
| Admin  | admin@nexus.io     | Admin@123    |
| Viewer | viewer@nexus.io    | Viewer@123   |

> These are seeded automatically on first run. Change them via the DB after launch.

---

## 🗄️ Database (SQLite)

The database file `nexus.db` is created automatically in the project root on first run using `EnsureCreated()` + the seed method in `Data/DbInitializer.cs`.

**Tables:**
- `LoginUsers` — Dashboard operators (auth users)
- `Users` — Managed user records (CRUD)
- `Transactions` — Financial transaction records

**Connection string** is in `appsettings.json`:
```json
"DefaultConnection": "Data Source=nexus.db"
```

---

## 🔑 Authentication

- Uses **ASP.NET Core Cookie Authentication** (no Identity overhead)
- Passwords hashed with **PBKDF2-SHA256** (100k iterations, 16-byte salt, 32-byte hash)
- Cookie expires in **8 hours** (30 days if "Keep me signed in" checked)
- Claims: `NameIdentifier`, `Name`, `Email`, `Role`

---

## 👮 Role-Based Access

| Action              | Admin | Viewer |
|---------------------|-------|--------|
| View Dashboard      | ✅    | ✅     |
| View Users          | ✅    | ✅     |
| Create/Edit/Delete  | ✅    | ❌     |
| View Transactions   | ✅    | ✅     |
| Add/Delete Tx       | ✅    | ❌     |

---

## 🎨 Design

Inspired by [OpsPulse – AI Ops & Compliance SaaS Dashboard](https://me.muz.li/orbix-studio/opspulse-ai-operations-compliance-saas-dashboard-design):
- Deep navy dark theme (`#080c18`)
- Electric blue + violet accent system
- Fine grid overlay with radial glows
- Glass-morphism cards with subtle borders
- Status dot indicators with CSS pulse animation
- Inter font, tabular-nums for data

---

## 🗂️ Project Structure

```
Controllers/
  AccountController.cs     ← Login / Logout
  DashboardController.cs   ← Home overview
  UserController.cs        ← User CRUD
  TransactionController.cs ← Transaction CRUD

Data/
  AppDbContext.cs          ← EF Core DbContext
  DbInitializer.cs         ← Seed data
  PasswordHelper.cs        ← PBKDF2 hashing

Models/
  LoginUser.cs             ← Auth user + LoginViewModel
  UserModel.cs             ← Managed user
  TransactionModel.cs      ← Transaction record

Views/
  Account/Login.cshtml     ← Standalone login page
  Dashboard/Index.cshtml
  User/{Index,Create,Edit}.cshtml
  Transaction/{Index,Create}.cshtml
  Shared/_Layout.cshtml    ← Dark SaaS shell
```

---

## ⚙️ Key Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore.Sqlite` | 9.0.0 | SQLite ORM |
| `Microsoft.EntityFrameworkCore.Design` | 9.0.0 | EF tooling |

No third-party auth libraries — pure ASP.NET Core built-ins.
