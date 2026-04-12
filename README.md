# Student System — ASP.NET Core MVC Dashboard

A functional student/user management dashboard with **SQLite persistence**, **cookie-based authentication**, and a clean **Bootstrap 5** light/dark theme.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

### Run the app
```bash
dotnet restore
dotnet run
```
Open the provided `localhost` URL in your browser.

---

## 🔐 Default Login Credentials

| Role   | Email              | Password     |
|--------|--------------------|--------------|
| Admin  | admin@nexus.io     | Admin@123    |
| Viewer | viewer@nexus.io    | Viewer@123   |

> **Note:** Passwords are hashed with **PBKDF2-SHA256**. The credentials are seeded in `Data/DbInitializer.cs`.

---

## 🗄️ Database (SQLite)

The system uses SQLite for data persistence. The database file `nexus.db` is managed via EF Core.

**Tables:**
- `LoginUsers` — System operators with different access levels.
- `Users` — Main user/student records (CRUD).
- `Transactions` — Activity/Financial logs.

**Connection string** (in `appsettings.json`):
```json
"DefaultConnection": "Data Source=nexus.db"
```

---

## 🔑 Key Features

- **Auth System**: Secure cookie-based authentication with PBKDF2 hashing.
- **Role Management**: 
  - **Admin**: Full CRUD access for Users and Transactions.
  - **Viewer**: Read-only access to records.
- **User CRUD**: Management of name, email, age, department, and status.
- **Transaction Logs**: Track and filter system activities or financial data.
- **Responsive UI**: Built with Bootstrap 5, featuring a persistent sidebar and modern dashboard aesthetic.
- **Theme Support**: Includes a built-in light/dark mode toggle.

---

## 🎨 Design & UI

- **Framework**: Bootstrap 5.3.3
- **Icons**: Bootstrap Icons 1.11.3
- **Layout**: Sidebar-driven navigation with a clean, responsive content area.
- **Validation**: Full server-side (ModelState) and client-side (jQuery Validation) support with clear visual feedback.

---

## 🗂️ Project Structure

```
Controllers/
  AccountController.cs     ← Login / Logout logic
  DashboardController.cs   ← Home overview & stats
  UserController.cs        ← User/Student CRUD
  TransactionController.cs ← Transaction management

Data/
  AppDbContext.cs          ← Entity Framework DbContext
  DbInitializer.cs         ← Database seeding logic
  PasswordHelper.cs        ← PBKDF2 hashing utility

Models/
  LoginUser.cs             ← Auth models
  UserModel.cs             ← User entity with Data Annotations
  TransactionModel.cs      ← Transaction record entity

Views/
  Account/Login.cshtml     ← Modern login page with failure memes
  Dashboard/Index.cshtml   ← Quick stats overview
  User/                    ← User management views
  Transaction/             ← Transaction history and creation
  Shared/_Layout.cshtml    ← Main dashboard layout with theme support
```
