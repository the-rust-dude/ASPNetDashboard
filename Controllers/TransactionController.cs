using ASPNetDashboard.Data;
using ASPNetDashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetDashboard.Controllers
{
    public class TransactionController : Controller
    {
        private string CurrentRole =>
            HttpContext.Session.GetString("Role") ?? "Guest";

        // ── Index / List ─────────────────────────────────────────────────

        public IActionResult Index(
            string? search,
            string? type,
            string? category,
            DateTime? fromDate,
            DateTime? toDate)
        {
            ViewBag.Role     = CurrentRole;
            ViewBag.Search   = search;
            ViewBag.Type     = type;
            ViewBag.Category = category;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate   = toDate?.ToString("yyyy-MM-dd");

            var txList = AppDataStore.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                txList = txList.Where(t =>
                    t.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(type))
                txList = txList.Where(t => t.Type == type);

            if (!string.IsNullOrWhiteSpace(category))
                txList = txList.Where(t =>
                    t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (fromDate.HasValue)
                txList = txList.Where(t => t.Date >= fromDate.Value);

            if (toDate.HasValue)
                txList = txList.Where(t => t.Date < toDate.Value.AddDays(1));

            var filtered = txList.OrderByDescending(t => t.Date).ToList();

            // Summary for filtered results
            ViewBag.FilteredCredit = filtered.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            ViewBag.FilteredDebit  = filtered.Where(t => t.Type == "Debit").Sum(t => t.Amount);

            // Totals for cards (always full set)
            ViewBag.TotalCredit = AppDataStore.TotalCredit;
            ViewBag.TotalDebit  = AppDataStore.TotalDebit;
            ViewBag.NetBalance  = AppDataStore.NetBalance;

            // Categories for filter dropdown
            ViewBag.Categories = AppDataStore.Transactions
                .Select(t => t.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(filtered);
        }

        // ── Create ───────────────────────────────────────────────────────

        public IActionResult Create()
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            ViewBag.Role = CurrentRole;
            return View(new TransactionModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransactionModel model)
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            ViewBag.Role = CurrentRole;

            if (ModelState.IsValid)
            {
                AppDataStore.AddTransaction(model);
                TempData["Success"] =
                    $"Transaction #{AppDataStore.Transactions.Last().Id} added successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
