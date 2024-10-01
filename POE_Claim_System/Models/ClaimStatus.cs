namespace POE_Claim_System.Models
{
    public class ClaimStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }  //pending, approved, rejected
        public DateTime timestamp { get; set; }
    }
}
