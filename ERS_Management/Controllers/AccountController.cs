using ERS_Management.Data;
using ERS_Management.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERS_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly ERS_ManagementContext _context;

        public AccountController(ERS_ManagementContext context) => _context = context;

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {


            if (!ModelState.IsValid) return View(model);

            var user = await _context.Account
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())  // Only username
// Only username
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {


            if (!ModelState.IsValid) return View(model);

            if (await _context.Account.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            var user = new Models.Account
            {
                Username = model.Username,
                Name = model.Name,
                Password = model.Password, // In production: hash this!
                Role = model.Role
            };

            _context.Account.Add(user);
            await _context.SaveChangesAsync();

            // Auto-login after register
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)  ,// Only username
                new Claim(ClaimTypes.Role, user.Role.ToString())  // Only username

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}

