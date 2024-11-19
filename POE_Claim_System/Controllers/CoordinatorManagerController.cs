using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using POE_Claim_System.Services; // Your service that handles claims

namespace POE_Claim_System.Controllers
{

    public class CoordinatorManagerController : Controller
    {
        private readonly ClaimService _claimService;

        public CoordinatorManagerController(ClaimService claimService)
        {
            _claimService = claimService;
        }



        // Display all pending claims for review
        public IActionResult Index()
        {
            var pendingClaims = _claimService.GetPendingClaims();
            return View(pendingClaims); // View to show all pending claims
        }

        // Approve a claim
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            _claimService.UpdateClaimStatus(claimId, "Approved");
            return RedirectToAction("Index"); // Redirect back to the pending claims list
        }

        // Reject a claim
        [HttpPost]
        public IActionResult RejectClaim(int claimId, string rejectReason)
        {
            /*
            if (string.IsNullOrWhiteSpace(rejectReason))
            {
                //error message if the reason is not provided.
                TempData["Error"] = "Rejection reason is required.";
                return RedirectToAction("Index");
            }
            */
            //_claimService.RejectClaim(claimId, rejectReason);

            _claimService.UpdateClaimStatus(claimId, "Rejected");
            return RedirectToAction("Index"); // Redirect back to the pending claims list
        }
    }
}
