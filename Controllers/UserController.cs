using ASPNetDashboard.Data;
using ASPNetDashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetDashboard.Controllers
{
    public class UserController : Controller
    {
        private string CurrentRole =>
            HttpContext.Session.GetString("Role") ?? "Guest";

        // ── Index / List ─────────────────────────────────────────────────

        public IActionResult Index(string? search, string? sortBy)
        {
            ViewBag.Role   = CurrentRole;
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;

            var users = AppDataStore.Users.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u =>
                    u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Sort
            users = sortBy switch
            {
                "name"  => users.OrderBy(u => u.Name),
                "age"   => users.OrderBy(u => u.Age),
                "date"  => users.OrderByDescending(u => u.CreatedAt),
                _       => users.OrderBy(u => u.Id)
            };

            return View(users.ToList());
        }

        // ── Create ───────────────────────────────────────────────────────

        public IActionResult Create()
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");
            ViewBag.Role = CurrentRole;
            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel model)
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            ViewBag.Role = CurrentRole;

            if (ModelState.IsValid)
            {
                // Check duplicate email
                if (AppDataStore.Users.Any(u =>
                    u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Email",
                        "This email address is already registered.");
                    return View(model);
                }

                AppDataStore.AddUser(model);
                TempData["Success"] = $"User '{model.Name}' was added successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ── Edit ─────────────────────────────────────────────────────────

        public IActionResult Edit(int id)
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            ViewBag.Role = CurrentRole;
            var user = AppDataStore.GetUser(id);
            if (user is null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel model)
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            ViewBag.Role = CurrentRole;

            // Password not re-validated on edit
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                // Duplicate email check (excluding self)
                if (AppDataStore.Users.Any(u =>
                    u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) &&
                    u.Id != model.Id))
                {
                    ModelState.AddModelError("Email",
                        "This email is already used by another user.");
                    return View(model);
                }

                AppDataStore.UpdateUser(model);
                TempData["Success"] = $"User '{model.Name}' was updated successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ── Delete ───────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (CurrentRole != "Admin")
                return RedirectToAction("Index");

            var user = AppDataStore.GetUser(id);
            if (user is not null)
            {
                AppDataStore.DeleteUser(id);
                TempData["Success"] = $"User '{user.Name}' was deleted.";
            }

            return RedirectToAction("Index");
        }
    }
}
