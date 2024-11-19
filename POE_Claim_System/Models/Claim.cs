namespace POE_Claim_System.Models
{
    public class Claim
    {
        public int Id { get; set; }
        //foreign keys courseid, personid
        public int TotalHours { get; set; }
        public double Rate { get; set; }

        public double TotalFee { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime Timestamp { get; set; }

        public string DocumentPath { get; set; }
        public int PersonId { get; set; }
        public int CourseId { get; set; }

        public int ClassId { get; set; }

        //claimstatus
        public int StatusId { get; set; }
        public DateTime DateClaimed { get; set; }

        //relationships
        public virtual Person Person { get; set; }
        public virtual Courses Course { get; set; }
        public string AdditionalNotes { get; set; }

        //public string RejectReason { get; set; }

    }

}
