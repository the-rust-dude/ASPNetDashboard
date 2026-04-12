using ASPNetDashboard.Models;

namespace ASPNetDashboard.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db)
        {
            db.Database.EnsureCreated();

            // ── Seed admin + viewer login users ─────────────────────────
            if (!db.LoginUsers.Any())
            {
                db.LoginUsers.AddRange(
                    new LoginUser
                    {
                        FullName = "System Administrator",
                        Email = "admin@nexus.io",
                        PasswordHash = PasswordHelper.HashPassword("Admin@123"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow.AddDays(-30)
                    },
                    new LoginUser
                    {
                        FullName = "Operations Viewer",
                        Email = "viewer@nexus.io",
                        PasswordHash = PasswordHelper.HashPassword("Viewer@123"),
                        Role = "Viewer",
                        CreatedAt = DateTime.UtcNow.AddDays(-15)
                    }
                );
            }

            // ── Seed managed users ───────────────────────────────────────
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new UserModel
                    {
                        Name = "Alice Santos",
                        Email = "alice@example.com",
                        PasswordHash = PasswordHelper.HashPassword("pass123"),
                        Age = 25,
                        Department = "Engineering",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    },
                    new UserModel
                    {
                        Name = "Bob Reyes",
                        Email = "bob@example.com",
                        PasswordHash = PasswordHelper.HashPassword("pass123"),
                        Age = 34,
                        Department = "Finance",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new UserModel
                    {
                        Name = "Carla Mendoza",
                        Email = "carla@example.com",
                        PasswordHash = PasswordHelper.HashPassword("pass123"),
                        Age = 28,
                        Department = "Operations",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new UserModel
                    {
                        Name = "Daniel Cruz",
                        Email = "daniel@example.com",
                        PasswordHash = PasswordHelper.HashPassword("pass123"),
                        Age = 31,
                        Department = "Marketing",
                        Status = "Inactive",
                        CreatedAt = DateTime.UtcNow.AddDays(-20)
                    },
                    new UserModel
                    {
                        Name = "Elena Torres",
                        Email = "elena@example.com",
                        PasswordHash = PasswordHelper.HashPassword("pass123"),
                        Age = 27,
                        Department = "Engineering",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    }
                );
            }

            // ── Seed transactions ────────────────────────────────────────
            if (!db.Transactions.Any())
            {
                db.Transactions.AddRange(
                    new TransactionModel
                    {
                        Description = "Laptop Purchase",
                        Amount = 45000,
                        Type = "Debit",
                        Category = "Equipment",
                        Status = "Completed",
                        Reference = "REF-2024-0001",
                        Date = DateTime.UtcNow.AddDays(-7)
                    },
                    new TransactionModel
                    {
                        Description = "Client Payment Received",
                        Amount = 120000,
                        Type = "Credit",
                        Category = "Income",
                        Status = "Completed",
                        Reference = "REF-2024-0002",
                        Date = DateTime.UtcNow.AddDays(-4)
                    },
                    new TransactionModel
                    {
                        Description = "Office Supplies",
                        Amount = 1850,
                        Type = "Debit",
                        Category = "Supplies",
                        Status = "Completed",
                        Reference = "REF-2024-0003",
                        Date = DateTime.UtcNow.AddDays(-2)
                    },
                    new TransactionModel
                    {
                        Description = "Software Subscription",
                        Amount = 2500,
                        Type = "Debit",
                        Category = "Software",
                        Status = "Pending",
                        Reference = "REF-2024-0004",
                        Date = DateTime.UtcNow.AddDays(-1)
                    },
                    new TransactionModel
                    {
                        Description = "Q4 Revenue Transfer",
                        Amount = 85000,
                        Type = "Credit",
                        Category = "Income",
                        Status = "Completed",
                        Reference = "REF-2024-0005",
                        Date = DateTime.UtcNow.AddHours(-3)
                    }
                );
            }

            db.SaveChanges();
        }
    }
}
