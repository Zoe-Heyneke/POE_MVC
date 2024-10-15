namespace POE_Claim_System.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string courseCode { get; set; }
        public int group {  get; set; }
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public DateTime timestamp { get; set; }
    }
}
