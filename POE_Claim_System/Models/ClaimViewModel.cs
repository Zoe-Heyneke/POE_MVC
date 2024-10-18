namespace POE_Claim_System.Models;

public class ClaimViewModel
{
    public int Id { get; set; }
    public int TotalHours { get; set; }
    public double Rate { get; set; }
    public double TotalFee { get; set; }
    public string DocumentPath { get; set; }
    public int PersonId { get; set; }
    public int CourseId { get; set; }
    public int ClassId { get; set; }
    public int StatusId { get; set; }
    public DateTime DateClaimed { get; set; }
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public string ClassName { get; set; }
    public string AdditionalNotes { get; set; }
    public string LectureFirstName { get; set; }
    public string LectureLastName { get; set; }
}