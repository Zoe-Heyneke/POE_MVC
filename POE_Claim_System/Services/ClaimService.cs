using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.IO; // Add for working with file streams
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
                _claimService.AddNewClaim(claim); // Use injected service

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
                new Course { Id = 1, Name = "Course 1" },  // Make sure the Course model has 'Name'
                new Course { Id = 2, Name = "Course 2" }
            };
        }
    }
}

/*
namespace POE_Claim_System.Services
{
    //declare context to be accessible
    ClaimsContext claimsContext;
    public class ClaimService()
    {
        public ClaimService()
        {
            claimsContext = new ClaimsContext();
        }
           
        public int AddNewClaim(Claim claim)
        {
            //logic to add to claim to db

            //how to get rate defined by lecturer to databse
            var rate = claimsContext.Rates.FirstOrDefault(x => x.PersonId == claim.PersonId);
            if (rate != null)
            {
                //attach claim
                claim.Rate = rate.HourlyRate;

                //exception if record not found
                double totalFee = claim.TotalHours * claim.Rate;
                claim.TotalFee = totalFee;
                claimsContext.Claims.Add(claim);
                claimsContext.SaveChanges();
                return claim.Id;
            }
            return 0;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to uodate claim to db
            //get record
            var _claim = claimsContext.Claims.FIrstOrDefault(x => x.Id == claim.Id);
            if(_claim != null)
            {
                double totalFee = claim.TotalHours * claim.Rate;
                _claim.TotalFee = totalFee;
                _claim.DateClaimed = claim.DateClaimed;
                _claim.ClassId = claim.ClassId;
                _claim.StatusId = claim.StatusId;
                //add more fields to update
                claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<Claim> GetAllClaimsForUser(int personId)
        {
            //search on db and return user claims
            //look for claim
            //create select statement
            var claims = claimsContext.Claims.Where(x => x.PersonId == personId).ToList();

            //return new List<ClaimService>();
            return claims.OrderByDescending(x =>  x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }

    }
}
*/
