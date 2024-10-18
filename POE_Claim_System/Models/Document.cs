namespace POE_Claim_System.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public DateTime Timestamp { get; set; }
        public int ClaimId { get; internal set; }
    }
}
