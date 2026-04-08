using ASPNetDashboard.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetDashboard.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            ViewBag.Role         = role;
            ViewBag.UserCount    = AppDataStore.Users.Count;
            ViewBag.TxCount      = AppDataStore.Transactions.Count;
            ViewBag.TotalCredit  = AppDataStore.TotalCredit;
            ViewBag.TotalDebit   = AppDataStore.TotalDebit;
            ViewBag.NetBalance   = AppDataStore.NetBalance;
            ViewBag.RecentUsers  = AppDataStore.Users
                                       .OrderByDescending(u => u.CreatedAt)
                                       .Take(5).ToList();
            ViewBag.RecentTx     = AppDataStore.Transactions
                                       .OrderByDescending(t => t.Date)
                                       .Take(5).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SetRole(string role)
        {
            var safeRole = role == "Admin" ? "Admin" : "Guest";
            HttpContext.Session.SetString("Role", safeRole);
            TempData["Info"] = $"You are now browsing as {safeRole}.";
            return RedirectToAction("Index");
        }
    }
}
