﻿using Microsoft.AspNetCore.Mvc;
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
        public LecturerController(ClaimService claimService, IWebHostEnvironment webHostEnvironment,
            ClaimsContext claimsContext)
        {
            claimsContext.Database.EnsureCreated();
            _claimService = claimService;
            _claimsContext = claimsContext;


            // Set the upload path to wwwroot/uploads
            _uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

            // Ensure the uploads folder exists
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        public IActionResult Index()
        {
            //fetch username from session
            var userName = HttpContext.Session.GetString("Username");

            //user validation when login
            if(string.IsNullOrEmpty(userName))
            {
                //if username null or empty redirect login action method in home controller
                return RedirectToAction("LogIn", "Home");
            }

            //fetch claims for lecturer using the valid username
            var claims = _claimService.GetAllClaimsForLecturer(userName);
            return View(claims);
        }


        [HttpGet]
        public IActionResult SubmitClaim()
        {
            var model = new ClaimView();
            _claimsContext.Database.EnsureCreated();
            var courses = _claimsContext.Courses.ToList();
            var classes = _claimsContext.Classes.ToList();
            model.Courses = new SelectList(courses, "Id", "Name");
            model.Classes = new SelectList(classes, "Id", "ClassName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(ClaimView model)
        {
            //add claim

            var userName = HttpContext.Session.GetString("Username");
            var user = _claimsContext.Persons.FirstOrDefault(x => x.EmailAddress == userName);
            if (ModelState.IsValid)
            {
                var claim = new Claim
                {
                    DateClaimed = DateTime.Now,
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now),
                    PersonId = user?.Id ?? 0,
                    CourseId = model.CourseId,
                    ClassId = model.ClassId,
                    StatusId = 1, //1 is pending
                    AdditionalNotes = model.AdditionalNotes,
                    TotalHours = model.TotalHours,
                };
                // claim.DateClaimed=DateTime.Now;

                // Handle file upload
                if (model.Document != null && model.Document.Length > 0)
                {
                    // Define the path where you want to save the file in wwwroot/uploads
                    var filePath = Path.Combine(_uploadFolderPath, model.Document.FileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Document.CopyToAsync(stream);
                    }

                    // Save the file path in the claim model (make sure this path is suitable for storage)
                    claim.DocumentPath = $"uploads/{model.Document.FileName}"; // Relative path to store in the database
                }

                // Save the claim using your service
                await _claimService.AddClaimAsync(claim); // Call the async method

                // Redirect to the view all their claims after successfully submitting the claim
                return RedirectToAction("Index");
            }

            // If there is an issue with the submission, return to the form 
            return View(model);
        }

        //insert track directory to view status

        /*
        public IActionResult Track(int id)
        {
            var username = User.Identity.Name;
            var claims = _claimService.GetAllClaimsForLecturer(username);
            return View(claims);
        }
        */
    }
}
