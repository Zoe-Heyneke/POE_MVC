using POE_Claim_System.Models;


namespace POE_Claim_System.Models
{
    public class ClaimView
    {
        public int CourseId { get; set; }
        public int TotalHours { get; set; }
        public double Rate { get; set; }
        public string AdditionalNotes { get; set; }
        public IFormFile Document { get; set; }
    }
}
