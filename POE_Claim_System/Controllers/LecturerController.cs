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

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                var claimService = new ClaimService();

                // Save the document to the server
                if (document != null && document.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot", "uploads", document.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }
                    claim.DocumentPath = filePath; // Add this field in Claim model
                }

                claimService.AddNewClaim(claim);
                return RedirectToAction("Index");
            }

            return View(claim);
        }


        public IActionResult SubmitClaim()
        {
            ViewBag.Courses = GetCourses(); // Assuming you have a method to fetch courses
            return View("~/Views/Home/SubmitClaim.cshtml");
        }

    }
}
