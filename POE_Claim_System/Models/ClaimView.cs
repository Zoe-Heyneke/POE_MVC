using Microsoft.AspNetCore.Mvc.Rendering;
using POE_Claim_System.Models;


namespace POE_Claim_System.Models
{
    public class ClaimView
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public double TotalHours { get; set; }
        public int ClassId { get; set; }    
       
        public string AdditionalNotes { get; set; }
        public IFormFile Document { get; set; }
        public SelectList? Courses { get; set; }

        public SelectList? Classes { get; set; }
    }
}
