namespace POE_Claim_System.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public int Group {  get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
