﻿namespace POE_Claim_System.Models
{
    public class Claim
    {
        public int Id { get; set; }
        //foreign keys courseid, personid
        public decimal totalHours { get; set; }
        public decimal rate { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public DateTime timestamp { get; set; }
    }
}
