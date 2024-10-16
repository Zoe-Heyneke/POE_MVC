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
            var model = GetPendingClaims(); // Fetch pending claims
            return View("~/Views/Home/CoordinatorManager.cshtml", model); // Specify the path to CoordinatorManager.cshtml
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
            _claimService.UpdateClaimStatus(claimId, "approved");
            return RedirectToAction("ReviewClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            _claimService.UpdateClaimStatus(claimId, "rejected");
            return RedirectToAction("ReviewClaims");
        }
    }
}
