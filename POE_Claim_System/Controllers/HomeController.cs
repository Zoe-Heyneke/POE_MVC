using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Http; // Required for Session

namespace POE_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimsContext _context;

        public HomeController(ClaimsContext context)
        {
            _context = context;
        }

        // Display the Sign-Up form
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(); // Create a SignUp.cshtml View
        }

        // Handle the Sign-Up submission
        [HttpPost]
        public IActionResult SignUp(string username, string password, string role)
        {
            if (!_context.Users.Any(u => u.Username == username))
            {
                var newUser = new User { Username = username, Password = password, Role = role };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.Error = "Username already exists.";
            return View();
        }

        // Display the Login form
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Create a Login.cshtml View
        }

        // Handle the Login submission
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Store user info in session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // Redirect based on role
                if (user.Role == "Lecturer")
                {
                    return RedirectToAction("Index", "Lecturer");
                }
                else if (user.Role == "Coordinator")
                {
                    return RedirectToAction("Index", "CoordinatorManager");
                }
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // Logout action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Login");
        }
    }
}
