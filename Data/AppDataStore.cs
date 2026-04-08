using ASPNetDashboard.Models;

namespace ASPNetDashboard.Data
{
    /// <summary>
    /// Static in-memory data store. Replace with DbContext for database persistence.
    /// </summary>
    public static class AppDataStore
    {
        private static int _nextUserId = 4;
        private static int _nextTransactionId = 5;

        private static readonly List<UserModel> _users = new()
        {
            new UserModel
            {
                Id = 1, Name = "Alice Santos", Email = "alice@example.com",
                Age = 25, Password = "pass123", ConfirmPassword = "pass123",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new UserModel
            {
                Id = 2, Name = "Bob Reyes", Email = "bob@example.com",
                Age = 34, Password = "pass123", ConfirmPassword = "pass123",
                CreatedAt = DateTime.Now.AddDays(-5)
            },
            new UserModel
            {
                Id = 3, Name = "Carla Mendoza", Email = "carla@example.com",
                Age = 28, Password = "pass123", ConfirmPassword = "pass123",
                CreatedAt = DateTime.Now.AddDays(-2)
            }
        };

        private static readonly List<TransactionModel> _transactions = new()
        {
            new TransactionModel
            {
                Id = 1, Description = "Laptop Purchase", Amount = 45000,
                Type = "Debit", Category = "Equipment",
                Date = DateTime.Now.AddDays(-7)
            },
            new TransactionModel
            {
                Id = 2, Description = "Client Payment Received", Amount = 120000,
                Type = "Credit", Category = "Income",
                Date = DateTime.Now.AddDays(-4)
            },
            new TransactionModel
            {
                Id = 3, Description = "Office Supplies", Amount = 1850,
                Type = "Debit", Category = "Supplies",
                Date = DateTime.Now.AddDays(-2)
            },
            new TransactionModel
            {
                Id = 4, Description = "Software Subscription", Amount = 2500,
                Type = "Debit", Category = "Software",
                Date = DateTime.Now.AddDays(-1)
            }
        };

        // ── User Operations ──────────────────────────────────────────────

        public static List<UserModel> Users => _users;

        public static void AddUser(UserModel user)
        {
            user.Id = _nextUserId++;
            user.CreatedAt = DateTime.Now;
            _users.Add(user);
        }

        public static UserModel? GetUser(int id) =>
            _users.FirstOrDefault(u => u.Id == id);

        public static void UpdateUser(UserModel updated)
        {
            var existing = _users.FirstOrDefault(u => u.Id == updated.Id);
            if (existing is null) return;
            existing.Name      = updated.Name;
            existing.Email     = updated.Email;
            existing.Age       = updated.Age;
        }

        public static void DeleteUser(int id) =>
            _users.RemoveAll(u => u.Id == id);

        // ── Transaction Operations ────────────────────────────────────────

        public static List<TransactionModel> Transactions => _transactions;

        public static void AddTransaction(TransactionModel t)
        {
            t.Id   = _nextTransactionId++;
            t.Date = DateTime.Now;
            _transactions.Add(t);
        }

        // ── Summary Helpers ───────────────────────────────────────────────

        public static decimal TotalCredit =>
            _transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);

        public static decimal TotalDebit =>
            _transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);

        public static decimal NetBalance => TotalCredit - TotalDebit;
    }
}
