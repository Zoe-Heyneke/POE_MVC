using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Http; // Required for session access

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;

        public LecturerController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Index()
        {
            // Ensure the user is logged in and is a Lecturer
            var role = HttpContext.Session.GetString("Role");
            if (role != "Lecturer")
            {
                return RedirectToAction("Login", "Home");
            }

            // Get claims for the logged-in lecturer
            var username = HttpContext.Session.GetString("Username");
            var claims = _claimService.GetAllClaimsForUser(username);
            return View(claims); // Create Index.cshtml for Lecturer
        }

        // Add other actions like SubmitClaim etc., similarly checking the session and role.
    }
}
