using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using POE_Claim_System.Services;

namespace POE_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimService _claimService;

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
            if (_claimService.ValidateUser(username, password, role))
            {
                return RedirectToAction("Index", role == "Lecturer" ? "Lecturer" : "CoordinatorManager");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View("Index");
        }
    }
}
