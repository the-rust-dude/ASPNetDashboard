using ASPNetDashboard.Data;
using ASPNetDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetDashboard.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db) => _db = db;

        // ── Index / Search ─────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? search, string? sortBy, string? status)
        {
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.Status = status;

            var q = _db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(u => u.Name.Contains(search) ||
                                 u.Email.Contains(search) ||
                                 u.Department.Contains(search));

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(u => u.Status == status);

            q = sortBy switch
            {
                "name"   => q.OrderBy(u => u.Name),
                "age"    => q.OrderBy(u => u.Age),
                "date"   => q.OrderByDescending(u => u.CreatedAt),
                "status" => q.OrderBy(u => u.Status),
                _        => q.OrderByDescending(u => u.CreatedAt)
            };

            return View(await q.ToListAsync());
        }

        // ── Create ─────────────────────────────────────────────────────────
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View(new UserModel());

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(UserModel model)
        {
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid) return View(model);

            if (await _db.Users.AnyAsync(u =>
                    u.Email.ToLower() == model.Email.ToLower()))
            {
                ModelState.AddModelError("Email",
                    "This email address is already registered.");
                return View(model);
            }

            model.PasswordHash = PasswordHelper.HashPassword(model.Password);
            model.CreatedAt    = DateTime.UtcNow;

            _db.Users.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"User '{model.Name}' added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Edit ───────────────────────────────────────────────────────────
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null) return NotFound();
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UserModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid) return View(model);

            var existing = await _db.Users.FindAsync(model.Id);
            if (existing is null) return NotFound();

            if (await _db.Users.AnyAsync(u =>
                    u.Email.ToLower() == model.Email.ToLower() &&
                    u.Id != model.Id))
            {
                ModelState.AddModelError("Email",
                    "This email is already used by another user.");
                return View(model);
            }

            existing.Name       = model.Name;
            existing.Email      = model.Email;
            existing.Age        = model.Age;
            existing.Department = model.Department;
            existing.Status     = model.Status;

            await _db.SaveChangesAsync();

            TempData["Success"] = $"User '{model.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Delete ─────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is not null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"User '{user.Name}' has been removed.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
