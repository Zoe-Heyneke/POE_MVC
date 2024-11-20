using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using POE_Claim_System.Services; // Your service that handles claims
//pdf
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection.PortableExecutable;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using POE_Claim_System.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace POE_Claim_System.Controllers
{

    public class CoordinatorManagerController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly string _uploadFolderPath;

        public CoordinatorManagerController(ClaimService claimService, IWebHostEnvironment webHostEnvironment)
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



        // Display all pending claims for review
        public IActionResult Index()
        {
            var pendingClaims = _claimService.GetPendingClaims();
            return View(pendingClaims); // View to show all pending claims
        }

        //approve a claim
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            _claimService.UpdateClaimStatus(claimId, "Approved");
            //update the session with the latest approved claims
            var appClaims = _claimService.GetApprovedClaims();
            //set convert claims to string
            HttpContext.Session.SetString("AppClaimsData", JsonConvert.SerializeObject(appClaims));

            return RedirectToAction("Index"); // Redirect back to the pending claims list
        }

        //reject a claim
        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            _claimService.UpdateClaimStatus(claimId, "Rejected");
            //_claimService.RejectClaim(claimId, rejectReason);
            return RedirectToAction("Index"); // Redirect back to the pending claims list
        }

        //let hrview method get approved claims
        public IActionResult HrView()
        {
            var approvedClaims = _claimService.GetApprovedClaims(); //get approved claims
            return View("HrView", approvedClaims); //in hrview
        }

        //generate invoice
        public IActionResult GenerateReportInvoice()
        {
            string fileName = "HR_Report_" + DateTime.Now.ToFileTime() + ".pdf";
            //get data from session
            var appClaimsData = HttpContext.Session.GetString("AppClaimsData");
            var appClaims = JsonConvert.DeserializeObject<List<ClaimViewModel>>(appClaimsData!);
            //now have a list of all claims

            //prepare file
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 30, 40);
            //pdf saved in uploads folder
            string uploadPath = _uploadFolderPath;
            var fullPath = Path.Combine(uploadPath, fileName);
            //create file on system
            var fs = new FileStream(fullPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            Font header = FontFactory.GetFont("Serif", 12, Font.BOLD, BaseColor.BLACK);
            Font data = FontFactory.GetFont("Serif", 10, Font.NORMAL, BaseColor.BLACK);
            document.Open();
            //add information in header
            document.Add(new Paragraph("Report: Claims Approved", header));
            document.Add(new Paragraph($"Date {DateTime.Now.ToString("dd MMM yyyy HH:mm")}", header));
            document.Add(new Paragraph($"Please see below a report of all approved claims valid for invoice capturing:", header));

            //set columns and width
            PdfPTable table = new PdfPTable(10);
            table.HorizontalAlignment = 0;
            table.WidthPercentage = 100;
            float[] widths = new float[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell();
            //id column
            cell.Phrase = new Phrase("ID", header);
            table.AddCell(cell);

            //code column
            cell = new PdfPCell();
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

            //lecturer first name column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Lecturer FirstName", header);
            table.AddCell(cell);

            //lecturer last name column
            cell = new PdfPCell();
            cell.Phrase = new Phrase("Lecturer LastName", header);
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

            //looking through data submitted in claim system
            foreach (var claim in appClaims!)
            {
                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.Id.ToString(), data);
                table.AddCell(cell);

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
                cell.Phrase = new Phrase(claim.LectureFirstName, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(claim.LectureLastName, data);
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
            }

            //add new cell to show total
            cell = new PdfPCell();
            cell.Colspan = 7;
            cell.Phrase = new Phrase("TOTAL AMOUNT FOR CLAIMS", header);
            table.AddCell(cell);

            //sum up all claims
            cell = new PdfPCell();
            cell.Colspan = 3;
            cell.Phrase = new Phrase(appClaims.Sum(x => x.TotalFee).ToString(), header);
            table.AddCell(cell);

            //format add space
            document.Add(new Paragraph(Environment.NewLine));
            //add table in document
            document.Add(table);

            //summary at end
            document.Add(new Paragraph(Environment.NewLine)); //add space 
            document.Add(new Paragraph("Report Summary:", FontFactory.GetFont("Serif", 12, Font.BOLD, BaseColor.BLACK)));
            document.Add(new Paragraph($"Total Claims: {appClaims.Count}", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)));
            document.Add(new Paragraph($"Total Amount: {appClaims.Sum(c => c.TotalFee):C}", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); 


            //close files to avoid error
            document.Close();
            writer.Close();
            fs.Close();

            return Redirect($"/uploads/{fileName}");
        }
    }
}
