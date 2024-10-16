using POE_Claim_System.Models;


namespace POE_Claim_System.Models
{
    public class ClaimView
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string LecturerName { get; set; }
        public string LecturerSurname { get; set; }
        public int CourseId { get; set; }
        public double TotalHours { get; set; }
        public double Rate { get; set; }
        public double TotalFee { get; set; }
        public DateTime DateClaimed { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string AdditionalNotes { get; set; }
        public IFormFile Document { get; set; }
    }
}
