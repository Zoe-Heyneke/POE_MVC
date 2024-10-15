namespace POE_Claim_System.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string IdNumber { get; set; }
        public string StaffNumber { get; set; }

        
        public string Role {  get; set; }
        public string Username { get; set; }
        public string Password {  get; set; }

        public DateTime Timestamp { get; set; }

        //link to claim
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
