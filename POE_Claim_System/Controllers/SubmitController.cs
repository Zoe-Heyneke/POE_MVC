using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;  // Assuming your Claim model is in this namespace
using POE_Claim_System.Services;  // Assuming your _claimService is here

public class SubmitController : Controller
{
    private readonly ClaimService _claimService;

    // Constructor injection for the claim service
    public SubmitController(ClaimService claimService)
    {
        _claimService = claimService;
    }

    [HttpPost]
    public IActionResult SubmitClaim(Claim claim)
    {
        if (ModelState.IsValid)
        {
            // Save the claim using your service or repository
            _claimService.AddClaim(claim);

            // Redirect to the ViewClaims method after successfully submitting the claim
            return RedirectToAction("ViewClaims", "View");
        }

        // If there is an issue with the submission, return to the form (with errors)
        return View("~/Views/Home/SubmitClaim.cshtml", claim);
    }
}
