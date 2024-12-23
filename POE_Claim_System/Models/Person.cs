﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_Claim_System.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string StaffNumber { get; set; }

        
        public string RoleName {  get; set; }
        public string Password {  get; set; }

        public DateTime Timestamp { get; set; }

        //link to claim
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
