namespace POE_Claim_System.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string contactNumber { get; set; }
        public string idNumber { get; set; }
        public string staffNumber { get; set; }

        public DateTime timestamp { get; set; }

        //link to claim
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
