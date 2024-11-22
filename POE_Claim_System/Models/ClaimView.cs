using Microsoft.AspNetCore.Mvc.Rendering;
using POE_Claim_System.Models;
using System.ComponentModel.DataAnnotations;


namespace POE_Claim_System.Models
{
    public class ClaimView
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Course is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Total hours worked is required.")]
        [Range(1, 60, ErrorMessage = "Total hours must be between 1 and 60.")]
        public int TotalHours { get; set; }

        [Required(ErrorMessage = "Group number is required.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Notes is required.")]
        public string AdditionalNotes { get; set; }

        [Required(ErrorMessage = "Document is required.")]
        public IFormFile Document { get; set; }
        public SelectList? Courses { get; set; }

        public SelectList? Classes { get; set; }
    }
}
