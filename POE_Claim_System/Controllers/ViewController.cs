using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services; // Assuming ClaimService is being used

public class ViewController : Controller
{
    private readonly ClaimService _claimService;

    public ViewController(ClaimService claimService)
    {
        _claimService = claimService;
    }

    public IActionResult ViewClaims()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Fetch claims for the logged-in user
            var claims = _claimService.GetAllClaimsForLecturer(User.Identity.Name);
            return View("ViewClaim", claims);
        }
        else
        {
            // No user logged in, so fetch all claims (or handle accordingly)
            var claims = _claimService.GetAllClaims(); // Fetch all or default set
            return View("ViewClaim", claims);
        }
    }

    /*
    public IActionResult ViewClaims()
    {
        var allClaims = _claimService.GetAllClaims(); // Modify the ClaimService to return all claims
        return View("ViewClaim", allClaims); // ViewClaim is the view you want to display the claims
    }
    */
}
