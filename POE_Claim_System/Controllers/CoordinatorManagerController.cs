using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class CoordinatorManagerController : Controller
    {
        private readonly ClaimService _claimService;

        public CoordinatorManagerController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Index()
        {
            // Ensure the user is logged in and is a Coordinator
            var role = HttpContext.Session.GetString("Role");
            if (role != "Coordinator")
            {
                return RedirectToAction("Login", "Home");
            }

            // Fetch pending claims for the coordinator to review
            var pendingClaims = _claimService.GetPendingClaims();
            return View(pendingClaims); // Create Index.cshtml for CoordinatorManager
        }

        private List<ClaimView> GetPendingClaims()
        {
            // Fetch pending claims from the database or your data source
            // This is a placeholder; replace it with your actual implementation
            return _claimService.GetPendingClaims(); // Example of calling a service method
        }

        public IActionResult ReviewClaims()
        {
            var pendingClaims = _claimService.GetPendingClaims();
            return View("~/Views/Home/ReviewClaims.cshtml", pendingClaims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            _claimService.ApproveClaim(claimId);
            return RedirectToAction("CoordinatorPage");
        }


        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            _claimService.UpdateClaimStatus(claimId, "rejected");
            return RedirectToAction("ReviewClaims");
        }
    }
}
