namespace POE_Claim_System.Models
{
    public class ClaimItemModel
    {
        public int Id { get; set; }
        public DateTime DateClaimed { get; set; }
        public string PersonName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public int TotalHours { get; set; }
        public double HourlyRate { get; set; }
        public double TotalFee { get; set; }
    }
}
