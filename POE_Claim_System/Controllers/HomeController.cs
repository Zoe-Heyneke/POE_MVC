using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; // Required for Session

namespace POE_Claim_System.Controllers
{

    public class HomeController : Controller
    {

        private readonly ClaimsContext _context;

        public HomeController(ClaimsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }


        public IActionResult LogIn()
        {
            return View();
        }


        public IActionResult Signup()
        {
            return View();
        }

        /*
        public IActionResult ViewClaim()
        {
            return View();
        }
        */
        [HttpPost]
        public IActionResult Signup(Person person)
        {
            if (_context.Users.Any(u => u.Username == person.EmailAddress))
            {
                ViewBag.Error = "Username already exists.";
                return View("LogIn");
            }
            person.Timestamp = DateTime.Now;
            _context.Persons.Add(person);
            _context.SaveChanges();
            var newUser = new User { Username = person.EmailAddress, RoleName = person.RoleName };
            newUser.Password = person.Password; // Hash the password

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return View("Index");
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("LogIn");
        }


        [HttpPost]
        public IActionResult LogIn(LogIn model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Store user info in session
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.RoleName);

                    // Redirect based on role
                    if (user.RoleName == "Lecturer")
                    {
                        return RedirectToAction("Index", "Lecturer");
                    }
                    else if (user.RoleName == "CoordinatorManager")
                    {
                        return RedirectToAction("Index", "CoordinatorManager");
                    }
                }

                ViewBag.Error = "Invalid username or password.";
            }

            return View(model);
        }
    }


}
