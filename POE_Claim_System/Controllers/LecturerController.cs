using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using POE_Claim_System.Services; // Your service that handles claims

namespace POE_Claim_System.Controllers
{
    [Authorize(Roles = "Lecturer")] // Only allow Lecturers to access this controller
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;

        public LecturerController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Index()
        {
            var username = HttpContext.User.Identity.Name; // Get the logged-in user's username (or ID)
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var claims = _claimService.GetAllClaimsForUser(username); // Retrieve claims for this user
            return View(claims); // Return claims to the view
        }
    }
}
