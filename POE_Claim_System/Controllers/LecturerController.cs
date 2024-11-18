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
            //set convert claims to string
            HttpContext.Session.SetString("ClaimsData", JsonConvert.SerializeObject(claims));
            return View(claims);
        }


        public IActionResult DownloadReport()
        {
            string fileName = DateTime.Now.ToFileTime() + ".pdf";
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
            document.Add(new Paragraph("Claims Report", header));
            document.Add(new Paragraph($"Date {DateTime.Now.ToString("dd MMM yyyy HH:mm")}", header));
            document.Add(new Paragraph($"Please see below a table view of all your claims submitted:", header));

            //set columns and width
            PdfPTable table = new PdfPTable(8);
            table.HorizontalAlignment = 0;
            table.WidthPercentage = 100;
            float[] widths = new float[] { 20, 20, 10, 10, 10, 10, 10, 10 };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell();
            //course column
            cell.Phrase = new Phrase("Course Code", header);
            table.AddCell(cell);

            //class column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Class Name", header);
            table.AddCell(cell);

            //group column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Group", header);
            table.AddCell(cell);


            //date column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Date Claimed", header);
            table.AddCell(cell);


            //hours column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Hours", header);
            table.AddCell(cell);


            //rate column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Rate", header);
            table.AddCell(cell);


            //total column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Total", header);
            table.AddCell(cell);


            //status column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Status", header);
            table.AddCell(cell);

            //looking through data submitted in claim system
            foreach (var claim in claims!)
            {
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseCode, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.CourseName, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.ClassName, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.DateClaimed.ToString("dd MMM yyyy"), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalHours.ToString(), data);
                table.AddCell(cell);
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Rate.ToString(), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.TotalFee.ToString(), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Status, data);
                table.AddCell(cell);
            }

            //add new cell to show total
            cell = new PdfPCell();
            cell.Colspan = 6;
            cell.Phrase = new Phrase("TOTAL AMOUNT FOR CLAIMS", header);
            table.AddCell(cell);

            //sum up all claims
            cell = new PdfPCell();
            cell.Colspan = 2;
            cell.Phrase = new Phrase(claims.Sum(x => x.TotalFee).ToString(), header);
            table.AddCell(cell);

            //format
            document.Add(new Paragraph(Environment.NewLine));
            //add table in document
            document.Add(table);

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
