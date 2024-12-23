﻿/*

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using POE_Claim_System.Models;  
using POE_Claim_System.Services;  
using System.IO;
using System.Threading.Tasks;

public class SubmitController : Controller
{
    private readonly ClaimService _claimService;
    private readonly string _uploadFolderPath;

    // Constructor injection for the claim service
    public SubmitController(ClaimService claimService, IWebHostEnvironment webHostEnvironment)
    {
        _claimService = claimService;

        // Set the upload path to wwwroot/uploads
        _uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

        // Ensure the uploads folder exists
        if (!Directory.Exists(_uploadFolderPath))
        {
            Directory.CreateDirectory(_uploadFolderPath);
        }
    }

    [HttpGet]
    public IActionResult SubmitClaim()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadDocument)
    {
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

            // Redirect to the ViewClaims method after successfully submitting the claim
            return RedirectToAction("ViewClaims", "View");
        }

        // If there is an issue with the submission, return to the form (with errors)
        return View("~/Views/Home/SubmitClaim.cshtml", claim);
    }

    public IActionResult Success()
    {
        return View(); 
    }
}
*/