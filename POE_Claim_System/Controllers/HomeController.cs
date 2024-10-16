using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using POE_Claim_System.Services;

namespace POE_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly ClaimsContext _context; // Replace with your actual DbContext

        public HomeController(ClaimService claimService, ClaimsContext context)
        {
            _claimService = claimService;
            _context = context; // Assign the context
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
            try
            {
                var user = _context.Persons
                .FirstOrDefault(p => p.Username == username && p.Password == password && p.Role == role);

                if (user != null)
                {
                    // Set user session or authentication cookie here if necessary

                    // Redirect based on role
                    if (user.Role == "Lecturer")
                    {
                        return RedirectToAction("Index", "Lecturer");
                    }
                    else if (user.Role == "CM") // For Coordinator and Manager
                    {
                        return RedirectToAction("Index", "Coordinator");
                    }
                }
                else
                {
                    // Handle login failure
                    ViewBag.ErrorMessage = "Invalid login attempt. Please try again.";
                    return View("Index");
                }
            }
            catch (Exception ex) 
            {
                // Log the exception (using a logging framework or Console)
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
            }
            return View("Index");

        }
    }
}
