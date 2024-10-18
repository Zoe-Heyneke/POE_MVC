using System.ComponentModel.DataAnnotations;

namespace POE_Claim_System.Models
{
    public class SignUp
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
