# 🚀 ASP.NET Core MVC Dashboard – Navigation Guide

This guide is designed to help you navigate the project with ease during your presentation. Use it as a quick reference when the professor asks about specific parts of the codebase.

---

## 🏗️ 1. The MVC Structure (The "Where is X?" Map)

### 📊 **Models (Data & Validation)**
*Location:* `Models/`
- **`UserModel.cs`**: Contains the **Data Annotations** (Required, Email, Range, etc.) for the User form. This is where your custom error messages are defined.
- **`TransactionModel.cs`**: Defines the properties for the transaction system (ID, Date, Description, Amount, Type, Category).

### ⚙️ **Controllers (Logic & Brains)**
*Location:* `Controllers/`
- **`UserController.cs`**: Handles User Registration, Editing, Deletion, and **Search** functionality. Look for the `Index` and `Create` methods.
- **`TransactionController.cs`**: Manages the **Filter logic** (by Category, Date, and Type). This is where the **LINQ** queries live.
- **`DashboardController.cs`**: Manages the main overview and the **Role Simulation** (`SetRole` action).

### 🎨 **Views (UI & Frontend)**
*Location:* `Views/`
- **`Shared/_Layout.cshtml`**: The **Main Sidebar**, top navigation, and common CSS/Scripts. This is the "Master Page."
- **`User/Create.cshtml`**: The **Registration Form** featuring Tag Helpers and validation messages.
- **`Transaction/Index.cshtml`**: The **Data Table** and the **Filter Bar**.
- **`Dashboard/Index.cshtml`**: The main page with the **Stat Cards** and summaries.

---

## 🛠️ 2. Key Technical Points (Common Questions)

### **"Where is the data stored?"**
- **File:** `Data/AppDataStore.cs`
- **Answer:** We use **Static In-Memory Lists** (`List<UserModel>` and `List<TransactionModel>`) to simulate a database. This allows data to persist as long as the application is running.

### **"How does Role-based Access work?"**
- **Logic:** `Controllers/DashboardController.cs` (uses `HttpContext.Session`).
- **UI:** `Views/Shared/_Layout.cshtml` uses `@if (currentRole == "Admin")` to show or hide the "Add User" and "New Transaction" links.
- **Security:** In `UserController.cs`, the `[HttpPost]` actions check the session role before saving.

### **"How is Search and Filtering implemented?"**
- **File:** `Controllers/TransactionController.cs` (inside the `Index` action).
- **Answer:** We capture the search query and filter values from the URL parameters and use **LINQ** (`.Where()`, `.Contains()`) to filter the static data before passing it to the View.

### **"How is validation configured?"**
- **Server-Side:** Show `if (ModelState.IsValid)` in the `Create` actions of the Controllers.
- **Client-Side:** Show the jQuery Validation scripts at the bottom of `_Layout.cshtml`.
- **Rules:** Show the `UserModel.cs` Data Annotations.

---

## 💡 3. Quick Navigation Tips
- **`Ctrl + ,`**: Quick Search for any file in Visual Studio.
- **`F12`**: Go to Definition (e.g., click a variable name and press F12 to see where it's defined).
- **`Program.cs`**: Show this if asked how **Session** is configured (`builder.Services.AddSession(...)`).

---

## 🎤 4. Suggested Presentation Flow
1. **Start at `Models/UserModel.cs`**: "This is where our data rules and validation are defined."
2. **Go to `Views/User/Create.cshtml`**: "Here is the form using Tag Helpers to apply those rules."
3. **Go to `Controllers/UserController.cs`**: "The controller validates the model before saving it to our memory store."
4. **End at `Views/Shared/_Layout.cshtml`**: "Our layout uses Bootstrap and Session-based roles to create a responsive, secure dashboard."
