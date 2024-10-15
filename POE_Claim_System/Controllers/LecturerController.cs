using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.IO; // For working with file streams
using Microsoft.AspNetCore.Http; // For IFormFile interface

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService; // Injected service

        // Constructor injection
        public LecturerController(ClaimService claimService)
        {
            _claimService = claimService; // Assign injected service
        }

        public IActionResult Index()
        {
            var claims = _claimService.GetAllClaimsForUser(1); // Use injected service
            return View(claims);
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                // Save the document to the server
                if (document != null && document.Length > 0)
                {
                    var uploadDir = Path.Combine("wwwroot", "uploads");

                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(document.FileName);
                    var filePath = Path.Combine(uploadDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }

                    claim.DocumentPath = Path.Combine("uploads", uniqueFileName);
                }

                _claimService.AddNewClaim(claim); // Use injected service

                return RedirectToAction("Index");
            }

            return View(claim); // If validation fails, return the same view with the model
        }

        // GET method to display the SubmitClaim form with available courses
        public IActionResult SubmitClaim()
        {
            ViewBag.Courses = GetCourses(); // Assuming you have a method to fetch courses
            return View("~/Views/Home/SubmitClaim.cshtml");
        }

        private List<Course> GetCourses()
        {
            return new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };
        }
    }
}
