using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Your service that handles claims
//pdf
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection.PortableExecutable;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


namespace POE_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly string _uploadFolderPath;

        ClaimsContext _claimsContext;

        //static dictionary to hold rejection reasons
        //private static readonly Dictionary<int, string> _rejectionReasons = new Dictionary<int, string>();

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

        //method to get reason from the static dictionary
        /*
        private string GetRejectionReason(int claimId)
        {
            //if the rejection reason exists in the static dictionary
            if (_rejectionReasons.TryGetValue(claimId, out var rejectionReason))
            {
                return rejectionReason;
            }
            return null; //return null if no reason is found to prevent error because approved doesnt get a reason
        }
        */

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

            // Add rejection reasons to claims from the static dictionary
            /*
            foreach (var claim in claims)
            {
                claim.RejectionReason = GetRejectionReason(claim.Id);
            }
            */

            //add rejection reasons to claims
            foreach (var claim in claims)
            {
                claim.RejectionReason = HttpContext.Session.GetString($"RejectionReason_{claim.Id}");
            }


            //set convert claims to string
            HttpContext.Session.SetString("ClaimsData", JsonConvert.SerializeObject(claims));
            return View(claims);
        }

        //download report of submitted claims
        public IActionResult DownloadReport()
        {
            string fileName = "Lecturer_Report_" + DateTime.Now.ToFileTime() + ".pdf";
            //get data from session
            var claimsData = HttpContext.Session.GetString("ClaimsData");
            var claims = JsonConvert.DeserializeObject<List<ClaimViewModel>>(claimsData!);
            //now have a list of all claims

            //prepare file
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 30, 40);
            //pdf saved in uploads folder
            string uploadPath = _uploadFolderPath;
            var fullPath = Path.Combine(uploadPath, fileName);
            //create file on system
            var fs = new FileStream(fullPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            Font header = FontFactory.GetFont("Serif", 10, Font.BOLD, BaseColor.BLACK);
            Font data = FontFactory.GetFont("Serif", 8, Font.NORMAL, BaseColor.BLACK);
            document.Open();
            //add information in header
            document.Add(new Paragraph("Report: Claims Submitted", header));
            document.Add(new Paragraph($"Date {DateTime.Now.ToString("dd MMM yyyy HH:mm")}", header));
            document.Add(new Paragraph($"Please see below a table view of all your claims submitted:", header));

            //format
            document.Add(new Paragraph(Environment.NewLine));

            //set columns and width
            PdfPTable appTable = new PdfPTable(8);
            appTable.HorizontalAlignment = 0;
            appTable.WidthPercentage = 100;
            float[] widths = new float[] { 20, 20, 10, 10, 10, 10, 10, 10 };
            appTable.SetWidths(widths);
            PdfPCell cell = new PdfPCell();
            //course column
            cell.Phrase = new Phrase("Course Code", header);
            appTable.AddCell(cell);

            //class column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Class Name", header);
            appTable.AddCell(cell);

            //group column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Group", header);
            appTable.AddCell(cell);


            //date column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Date Claimed", header);
            appTable.AddCell(cell);


            //hours column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Hours", header);
            appTable.AddCell(cell);


            //rate column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Rate", header);
            appTable.AddCell(cell);


            //total column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Total", header);
            appTable.AddCell(cell);


            //status column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Status", header);
            appTable.AddCell(cell);

            var approvedClaims = claims.Where(c => c.StatusId == 2).ToList();

            //looking through data submitted in claim system
            foreach (var claim in approvedClaims)
            {
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseCode, data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseName, data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.ClassName, data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.DateClaimed.ToString("dd MMM yyyy"), data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalHours.ToString(), data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Rate.ToString(), data);
                appTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalFee.ToString(), data);
                appTable.AddCell(cell);

                //get correct status based on StatusId
                string statusText = claim.StatusId switch
                {
                    1 => "Pending",
                    2 => "Approved",
                    3 => "Rejected"
                };

                cell = new PdfPCell();
                cell.Phrase = new Phrase(statusText, data);
                appTable.AddCell(cell);

            }

            document.Add(new Paragraph("Approved Claims", header));
            document.Add(appTable);
            document.Add(new Paragraph($"Total Amount for Approved Claims: {approvedClaims.Sum(c => c.TotalFee):C}", header));

            //format
            document.Add(new Paragraph(Environment.NewLine));

            //rejected claims in table
            PdfPTable rejectedTable = new PdfPTable(8);
            rejectedTable.HorizontalAlignment = 0;
            rejectedTable.WidthPercentage = 100;
            rejectedTable.SetWidths(widths);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Course Code", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Class Name", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase(" Group", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Date Claimed", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Hours", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Rate", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Total", header);
            rejectedTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Status", header);
            rejectedTable.AddCell(cell);

            var rejectedClaims = claims.Where(c => c.StatusId == 3).ToList();
            foreach (var claim in rejectedClaims)
            {
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseCode, data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseName, data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.ClassName, data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.DateClaimed.ToString("dd MMM yyyy"), data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalHours.ToString(), data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Rate.ToString(), data);
                rejectedTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalFee.ToString(), data);
                rejectedTable.AddCell(cell);

                string statusText = claim.StatusId switch
                {
                    1 => "Pending",
                    2 => "Approved",
                    3 => "Rejected"
                };

                cell = new PdfPCell();
                cell.Phrase = new Phrase(statusText, data);
                rejectedTable.AddCell(cell);
            }

            document.Add(new Paragraph("Rejected Claims", header));
            document.Add(rejectedTable);

            //format
            document.Add(new Paragraph(Environment.NewLine));

            //pending claims in table
            PdfPTable pendingTable = new PdfPTable(8);
            pendingTable.HorizontalAlignment = 0;
            pendingTable.WidthPercentage = 100;
            pendingTable.SetWidths(widths);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Course Code", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Class Name", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Group", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Date Claimed", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Hours", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Rate", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Total", header);
            pendingTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Status", header);
            pendingTable.AddCell(cell);

            var pendingClaims = claims.Where(c => c.StatusId == 1).ToList();
            foreach (var claim in pendingClaims)
            {
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseCode, data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseName, data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.ClassName, data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.DateClaimed.ToString("dd MMM yyyy"), data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalHours.ToString(), data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Rate.ToString(), data);
                pendingTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalFee.ToString(), data);
                pendingTable.AddCell(cell);

                string statusText = claim.StatusId switch
                {
                    1 => "Pending",
                    2 => "Approved",
                    3 => "Rejected"
                };

                cell = new PdfPCell();
                cell.Phrase = new Phrase(statusText, data);
                pendingTable.AddCell(cell);
            }

            document.Add(new Paragraph("Pending Claims", header));
            document.Add(pendingTable);

            //close files to avoid error
            document.Close();
            writer.Close();
            fs.Close();

            return Redirect($"/uploads/{fileName}");
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

            //course validation to be required
            if (model.CourseId == 0)
            {
                ModelState.AddModelError("CourseId", "Please select a course.");
            }


            //hours validation
            if (model.TotalHours <= 0 || model.TotalHours > 60)
            {
                ModelState.AddModelError("TotalHours", "Total hours must be between 1 and 60.");
            }

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

        //edit claim by GET method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the claim from the database
            var claim = _claimsContext.Claims
                .Where(c => c.Id == id)
                .Select(c => new ClaimViewModel
                {
                    Id = c.Id,
                    ClassId = c.ClassId,
                    CourseId = c.CourseId,
                    TotalHours = c.TotalHours,
                    AdditionalNotes = c.AdditionalNotes,
                    Rate = c.Rate,
                    TotalFee = c.TotalFee,
                })
                .FirstOrDefault();

            if (claim == null)
            {
                return NotFound(); // Return a 404 if the claim is not found
            }

            // Populate the dropdowns for courses and classes
            claim.Courses = new SelectList(_claimsContext.Courses.ToList(), "Id", "Name", claim.CourseId);
            claim.Classes = new SelectList(_claimsContext.Classes.ToList(), "Id", "ClassName", claim.ClassId);

            return View(claim);
        }

        //edit claim by POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                //get the existing claim
                var claim = await _claimsContext.Claims.FindAsync(model.Id);
                if (claim == null)
                {
                    return NotFound(); //if not found return error
                }

                //update the claim information
                claim.ClassId = model.ClassId;
                claim.CourseId = model.CourseId;
                claim.TotalHours = model.TotalHours;
                claim.AdditionalNotes = model.AdditionalNotes;

                //calculate the total fee based on the updated hours and rate
                var rate = await _claimsContext.Rates.FirstOrDefaultAsync();
                if (rate != null)
                {
                    claim.Rate = rate.HourlyRate;
                    claim.TotalFee = claim.TotalHours * claim.Rate;
                }

                //save changes to the database
                await _claimsContext.SaveChangesAsync();
                return RedirectToAction("Index"); //redirect to the index page after saving
            }

            //redisplay form
            return View(model);
        }

        //delete by Get method
        [HttpGet]
        public IActionResult Delete(int id)
        {
            //get claim from the database by id
            var claim = _claimsContext.Claims
            .Where(c => c.Id == id)
            .Select(c => new ClaimViewModel
            {
                Id = c.Id,
                CourseId = c.CourseId,
                CourseName = c.Course.Name,
                ClassId = c.ClassId,
                TotalHours = c.TotalHours,
                Rate = c.Rate,
                TotalFee = c.TotalFee,
                DateClaimed = c.DateClaimed,
                AdditionalNotes = c.AdditionalNotes,
                StatusId = c.StatusId
            })
            .FirstOrDefault();

            if (claim == null)
            {
                return NotFound(); //return not found
            }

            return View(claim);
        }

        //delete by POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var claim = await _claimsContext.Claims.FindAsync(id);
            if (claim != null)
            {
                //remove the claim from the context
                _claimsContext.Claims.Remove(claim);
                await _claimsContext.SaveChangesAsync(); //then save changes to the database
            }

            //redirect to the index after deletion
            return RedirectToAction("Index");
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
