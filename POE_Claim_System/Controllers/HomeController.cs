using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using POE_Claim_System.Services;

namespace POE_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly ClaimsContext _context;

        public HomeController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult SignUp(Person person)
        {
            if (ModelState.IsValid)
            {
                _claimService.AddPerson(person); // Save person to DB

                return RedirectToAction("Index", person.Role == "Lecturer" ? "Lecturer" : "CoordinatorManager");
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult LogIn(string username, string password, string role)
        {
            var user = _context.Persons
                .FirstOrDefault(p => p.Username == username && p.Password == password && p.Role == role);

            if (user != null)
            {
                // Set user session or authentication cookie here if necessary

                // Redirect based on role
                if (user.Role == "Lecturer")
                {
                    return RedirectToAction("Index", "Lecturer"); // Assuming Index method in LecturerController
                }
                else if (user.Role == "CM") // For Coordinator and Manager
                {
                    return RedirectToAction("Index", "Coordinator"); // Assuming Index method in CoordinatorController
                }
            }

            // Handle login failure
            ViewBag.ErrorMessage = "Invalid login attempt. Please try again.";
            return View("Index"); // Return to Index page with error message
        }

    }
}
