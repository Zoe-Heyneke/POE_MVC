using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ClaimsContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public SignUpController(ClaimsContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // SignUp view
        }

        [HttpPost]
        public IActionResult SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ViewBag.Error = "Username already exists.";
                    return View(model);
                }

                var newUser = new User { Username = model.Username, Role = model.Role };
                newUser.Password = _passwordHasher.HashPassword(newUser, model.Password); // Hash the password

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index", "LogIn");
            }

            return View(model);
        }
    }

}
