using System.ComponentModel.DataAnnotations;

namespace POE_Claim_System.Models
{
    public class LogIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
