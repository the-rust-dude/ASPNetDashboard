using ASPNetDashboard.Data;
using ASPNetDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetDashboard.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly AppDbContext _db;
        public TransactionController(AppDbContext db) => _db = db;

        // ── Index ──────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? search, string? type, string? category)
        {
            ViewBag.Search   = search;
            ViewBag.Type     = type;
            ViewBag.Category = category;

            var categories = await _db.Transactions
                .Select(t => t.Category).Distinct().OrderBy(c => c).ToListAsync();
            ViewBag.Categories = categories;

            var q = _db.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(t => t.Description.Contains(search) ||
                                 t.Reference.Contains(search));

            if (!string.IsNullOrWhiteSpace(type))
                q = q.Where(t => t.Type == type);

            if (!string.IsNullOrWhiteSpace(category))
                q = q.Where(t => t.Category == category);

            return View(await q.OrderByDescending(t => t.Date).ToListAsync());
        }

        // ── Create ─────────────────────────────────────────────────────────
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View(new TransactionModel());

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TransactionModel model)
        {
            if (!ModelState.IsValid) return View(model);

            model.Date      = DateTime.UtcNow;
            model.Reference = $"REF-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";

            _db.Transactions.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Transaction '{model.Description}' recorded successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Delete ─────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var tx = await _db.Transactions.FindAsync(id);
            if (tx is not null)
            {
                _db.Transactions.Remove(tx);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Transaction '{tx.Description}' has been removed.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
