using ASPNetDashboard.Data;
using ASPNetDashboard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASPNetDashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db) => _db = db;

        // ── GET /Account/Login ────────────────────────────────────────────
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        // ── POST /Account/Login ───────────────────────────────────────────
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                model.FailedAttempts++;
                ModelState.Remove(nameof(model.FailedAttempts));
                return View(model);
            }

            var user = await _db.LoginUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());

            if (user is null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
            {
                model.FailedAttempts++;
                ModelState.Remove(nameof(model.FailedAttempts));
                ModelState.AddModelError(string.Empty,
                    "Invalid email or password. Please try again.");
                return View(model);
            }

            // Update last login timestamp
            user.LastLoginAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // Build claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc   = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProps);

            TempData["Success"] = $"Welcome back, {user.FullName}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        // ── POST /Account/Logout ──────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ── GET /Account/AccessDenied ─────────────────────────────────────
        [AllowAnonymous]
        public IActionResult AccessDenied() => RedirectToAction("Login");
    }
}
