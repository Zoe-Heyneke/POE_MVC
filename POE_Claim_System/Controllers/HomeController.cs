using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Person person)
        {
            // Add logic to save the person details to the database

            // Redirect based on the role
            if (person.Role == "Lecturer")
            {
                return RedirectToAction("Index", "Lecturer");
            }
            else if (person.Role == "Coordinator" || person.Role == "Manager")
            {
                return RedirectToAction("Index", "CoordinatorManager");
            }

            return View("Index"); // Fallback if needed
        }

        [HttpPost]
        public IActionResult LogIn(string username, string password, string role)
        {
            // Validate user credentials

            // Redirect based on the role
            if (role == "Lecturer")
            {
                return RedirectToAction("Index", "Lecturer");
            }
            else if (role == "Coordinator" || role == "Manager")
            {
                return RedirectToAction("Index", "CoordinatorManager");
            }

            return View("Index"); // Fallback if needed
        }
    }
}
