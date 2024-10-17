using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class LoginController : Controller
    {
        private readonly ClaimsContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(ClaimsContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Login view
        }

        [HttpPost]
        public IActionResult LogIn(LogIn model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Success)
                {
                    // Store user info in session
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    // Redirect based on role
                    if (user.Role == "Lecturer")
                    {
                        return RedirectToAction("SubmitClaim", "Lecturer");
                    }
                    else if (user.Role == "Coordinator")
                    {
                        return RedirectToAction("ViewClaim", "CoordinatorManager");
                    }
                }

                ViewBag.Error = "Invalid username or password.";
            }

            return View(model);
        }

    }

}
