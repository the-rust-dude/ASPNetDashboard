# 🛠️ ASPNetDashboard - Setup & Getting Started

Welcome to the **ASP.NET Core MVC Dashboard**! This guide will help you get the project up and running in minutes.

---

## 🏗️ 1. Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK**: [Download .NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) or higher.
- **IDE**: [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (v17.4 or later) or [Visual Studio Code](https://code.visualstudio.com/) with the C# extension.
- **Git**: (Optional) For version control.

---

## 🚀 2. Quick Start (Command Line)

1. **Clone or Download**: Ensure you're in the project root directory.
2. **Restore Dependencies**:
   ```powershell
   dotnet restore
   ```
3. **Build the Project**:
   ```powershell
   dotnet build
   ```
4. **Run the Application**:
   ```powershell
   dotnet run
   ```
   The application will be available at: `https://localhost:7289` (or the port specified in your output).

---

## 📂 3. Project Architecture & Navigation

This project follows the **MVC (Model-View-Controller)** pattern:

- **📊 Models (`/Models`)**: Contains data definitions and **Data Annotations** (validation rules).
- **⚙️ Controllers (`/Controllers`)**: Handles the application logic and routing.
- **🎨 Views (`/Views`)**: Contains the HTML/Razor templates styled with **Bootstrap 5**.
- **💾 Data Store (`/Data`)**: Uses a **Static In-Memory List** (`AppDataStore.cs`) to simulate a database.

---

## 🔑 4. Features to Explore

- **Dashboard**: Use the sidebar to switch between Guest and Admin roles (simulated).
- **User Management**: Add, edit, and delete users. Try submitting the form with invalid data to see **validation** in action.
- **Transactions**: View and filter transaction records. Search by description or filter by date/category.
- **Responsive UI**: The layout uses a custom sidebar and Bootstrap for a professional dashboard experience.

---

## 🛠️ 5. Development Tips

- **Validation Rules**: If you want to change form requirements, edit the properties in `/Models/UserModel.cs`.
- **Static Data**: Initial data is seeded in `Data/AppDataStore.cs`. Restarting the app will reset any changes made to the data.
- **Session Settings**: The session timeout and cookie configuration can be found in `Program.cs`.

---

## 🛑 6. Troubleshooting

- **Port in Use**: If you get a port error, check `Properties/launchSettings.json` to change the `applicationUrl`.
- **CSS Not Loading**: Ensure `wwwroot/css` contains the required static files and that `app.UseStaticFiles()` is in `Program.cs`.
- **Session Issues**: Ensure your browser accepts cookies, as roles are stored in the session.
