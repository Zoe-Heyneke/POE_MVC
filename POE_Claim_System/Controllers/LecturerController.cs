using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.IO; // Add for working with file streams
using Microsoft.AspNetCore.Http; // For IFormFile interface


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
                    // Define the upload directory path within wwwroot/uploads
                    var uploadDir = Path.Combine("wwwroot", "uploads");

                    // Ensure the directory exists, if not, create it
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Create a unique file name to avoid overwriting existing files
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(document.FileName);
                    var filePath = Path.Combine(uploadDir, uniqueFileName);

                    // Save the document to the specified file path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }

                    // Save the relative file path (starting after wwwroot) in the claim
                    claim.DocumentPath = Path.Combine("uploads", uniqueFileName);
                }

                // Add the claim to the service (or database)
                claimService.AddNewClaim(claim);

                // Redirect back to the Index page after successful submission
                return RedirectToAction("Index");
            }

            // If validation fails, return the same view with the model
            return View(claim);
        }


        // GET method to display the SubmitClaim form with available courses
        public IActionResult SubmitClaim()
        {
            ViewBag.Courses = GetCourses(); // Assuming you have a method to fetch courses
            return View("~/Views/Home/SubmitClaim.cshtml");
        }

        // Sample method to get available courses, you can replace this with your actual service
        private List<Course> GetCourses()
        {
            // Simulated fetching of courses, replace this with actual service logic
            return new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };
        }

    }
}
