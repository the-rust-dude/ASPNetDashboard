using ASPNetDashboard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetDashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var users        = await _db.Users.ToListAsync();
            var transactions = await _db.Transactions.ToListAsync();

            ViewBag.UserCount   = users.Count;
            ViewBag.TxCount     = transactions.Count;
            ViewBag.TotalCredit = transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            ViewBag.TotalDebit  = transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);
            ViewBag.NetBalance  = ViewBag.TotalCredit - ViewBag.TotalDebit;
            ViewBag.ActiveUsers = users.Count(u => u.Status == "Active");

            ViewBag.RecentUsers = users
                .OrderByDescending(u => u.CreatedAt)
                .Take(5).ToList();

            ViewBag.RecentTx = transactions
                .OrderByDescending(t => t.Date)
                .Take(5).ToList();

            // Activity stats for mini chart (last 7 days credits)
            ViewBag.WeeklyCredits = Enumerable.Range(0, 7)
                .Select(i => transactions
                    .Where(t => t.Type == "Credit" &&
                                t.Date.Date == DateTime.UtcNow.Date.AddDays(-i))
                    .Sum(t => t.Amount))
                .Reverse()
                .ToList();

            return View();
        }
    }
}
