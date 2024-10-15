using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Services;

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult Index()
        {
            ClaimService claimService = new ClaimService();
            var claims = claimService.GetAllClaimsForUser(1);//get the profile that is logged in
            return View(claims);
        }
    }
}
