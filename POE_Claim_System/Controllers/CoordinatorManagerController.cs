using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;

namespace POE_Claim_System.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ClaimService _claimService;

        public CoordinatorController(ClaimService claimService)
        {
            _claimService = claimService(); //injected service
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
