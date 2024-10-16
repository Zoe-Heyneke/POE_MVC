using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.Collections.Generic;
using System.IO; // For working with file streams
using Microsoft.AspNetCore.Http; // For IFormFile interface
//using POE_Claim_System.; // Import your ViewModels namespace

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService; // Injected service
        private readonly ClaimsContext _context; // Add database context

        // Constructor injection
        public LecturerController(ClaimService claimService, ClaimsContext context)
        {
            _claimService = claimService; // Assign injected service
            _context = context;           // Assign injected context
        }
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
        public async Task<IActionResult> SubmitClaim(ClaimView model, IFormFile uploadDocument)
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
                    // Other properties from the model
                };

                // Handle file upload
                if (uploadDocument != null && uploadDocument.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/documents", Path.GetFileName(uploadDocument.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadDocument.CopyToAsync(stream);
                    }
                    claim.DocumentPath = filePath; // Save the file path in the claim
                }

                // Save the claim to the database
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success"); // Redirect on success
            }

            // Repopulate ViewBag in case of error
            ViewBag.Courses = GetCourses();
            return View(model); // Return the view with validation errors
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
