using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;
using POE_Claim_System.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; // Include if you're using DbContext

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly ClaimsContext _context; // Added to access the database context

        public LecturerController(ClaimService claimService, ClaimsContext context)
        {
            _claimService = claimService;
            _context = context; // Initialize context
        }

        // Index action to display all claims for the logged-in user
        public IActionResult Index()
        {
            var username = User.Identity.Name;

            // Get claims for the logged-in user using username
            var claims = _claimService.GetAllClaimsForUser(username);
            return View(claims);
        }

        // Action to display the form for submitting a claim
        public IActionResult SubmitClaim()
        {
            var model = new ClaimView();
            ViewBag.Courses = GetCourses(); // Load available courses
            return View(model);
        }

        // POST action to submit a claim
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(ClaimView model, IFormFile uploadDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var username = GetCurrentUsername(); // Get the current user's username

                    var claim = new Claim
                    {
                        CourseId = model.CourseId,
                        TotalHours = (int)model.TotalHours,
                        Rate = model.Rate,
                        AdditionalNotes = model.AdditionalNotes,
                        Username = username // Set the username directly
                    };

                    // Handle file upload
                    if (uploadDocument != null && uploadDocument.Length > 0)
                    {
                        var filePath = Path.Combine("wwwroot/documents", Path.GetFileName(uploadDocument.FileName));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadDocument.CopyToAsync(stream);
                        }
                        claim.DocumentPath = filePath; // Set the document path in the claim
                    }

                    // Add the claim to the service
                    await _claimService.AddClaimAsync(claim);
                    return RedirectToAction("Success");
                }
                catch (UnauthorizedAccessException ex)
                {
                    ModelState.AddModelError("", ex.Message); // Add the error message to the ModelState
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("", ex.Message); // Add the error message to the ModelState
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred."); // General error message
                }
            }

            // Reload courses in case of validation errors
            ViewBag.Courses = GetCourses();
            return View(model);
        }



        // Helper method to get a list of courses
        private List<Course> GetCourses() => new List<Course>
        {
            new Course { Id = 1, Name = "Course 1" },
            new Course { Id = 2, Name = "Course 2" }
        };

        // Helper method to get the current user's ID based on the username
        private string GetCurrentUsername()
        {
            var username = User.Identity.Name; // Get the username from the claims

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return username; // Return username directly
        }

    }
}
