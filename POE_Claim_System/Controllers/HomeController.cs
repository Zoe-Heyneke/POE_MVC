using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Http; // Required for Session

namespace POE_Claim_System.Controllers
{
   
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }

        public IActionResult ViewClaim()
        {
            return View();
        }
    }
    

}
