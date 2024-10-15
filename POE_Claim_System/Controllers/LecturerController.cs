using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.IO; // For working with file streams
using Microsoft.AspNetCore.Http; // For IFormFile interface
//using POE_Claim_System.; // Import your ViewModels namespace

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

        // GET method to display the SubmitClaim form with available courses
        public IActionResult SubmitClaim()
        {
            var model = new ClaimView(); // Create a new instance of ClaimViewModel
            ViewBag.Courses = GetCourses(); // Populate your ViewBag with course options
            return View(model); // Return the view with the model
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(ClaimView model) // Change parameter type to ClaimViewModel
        {
            if (ModelState.IsValid)
            {
                // Create a new Claim object from the model
                var claim = new Claim
                {
                    CourseId = model.CourseId,
                    TotalHours = model.TotalHours,
                    Rate = model.Rate,
                    AdditionalNotes = model.AdditionalNotes,
                    // Set other properties as needed
                };

                // Handle the document upload
                if (model.Document != null && model.Document.Length > 0)
                {
                    var uploadDir = Path.Combine("wwwroot", "uploads");

                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Document.FileName);
                    var filePath = Path.Combine(uploadDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Document.CopyToAsync(stream); // Use CopyToAsync for async file copy
                    }

                    claim.DocumentPath = Path.Combine("uploads", uniqueFileName); // Set the document path
                }

                // Save the claim using the injected service
                _claimService.AddNewClaim(claim);

                return RedirectToAction("Index"); // Redirect to Index after successful claim submission
            }

            // Repopulate the ViewBag for the courses in case of a validation error
            ViewBag.Courses = GetCourses();
            return View(model); // Return the model with validation errors
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
