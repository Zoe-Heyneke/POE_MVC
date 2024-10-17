using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Your service that handles claims

namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly string _uploadFolderPath;

        ClaimsContext _claimsContext;

        // Constructor injection for the claim service
        public LecturerController(ClaimService claimService, IWebHostEnvironment webHostEnvironment, ClaimsContext claimsContext)
        {
            _claimService = claimService;
            _claimsContext = claimsContext;
            _claimsContext.Database.EnsureCreated();

            // Set the upload path to wwwroot/uploads
            _uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

            // Ensure the uploads folder exists
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        public IActionResult Index() { return View(); }


        [HttpGet]
        public IActionResult SubmitClaim()
        {
            var model = new ClaimView();

            var courses = _claimsContext.Courses.ToList();
            var classes = _claimsContext.Classes.ToList();
            model.Courses = new SelectList(courses, "Id", "Name");
            model.Classes = new SelectList(classes, "Id", "ClassName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadDocument)
        {
            //add claim


            if (ModelState.IsValid)
            {
                // Handle file upload
                if (uploadDocument != null && uploadDocument.Length > 0)
                {
                    // Define the path where you want to save the file in wwwroot/uploads
                    var filePath = Path.Combine(_uploadFolderPath, uploadDocument.FileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadDocument.CopyToAsync(stream);
                    }

                    // Save the file path in the claim model (make sure this path is suitable for storage)
                    claim.DocumentPath = $"uploads/{uploadDocument.FileName}"; // Relative path to store in the database
                }

                // Save the claim using your service
                await _claimService.AddClaimAsync(claim); // Call the async method

                // Redirect to the view all their claims after successfully submitting the claim
                return RedirectToAction("~/Views/Home/Lecturer/Index.cshtml", claim);
            }

            // If there is an issue with the submission, return to the form 
            return View();
        }
    }
}
