using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POE_Claim_System.Services
{
    public class ClaimService
    {
        private readonly ClaimsContext _claimsContext;
        private List<Claim> claims;

        public ClaimService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext; // Injected via DI
            claimsContext.Database.EnsureCreated();
        }


        public List<ClaimViewModel> GetAllClaims()
        {
            var claims = (from c in _claimsContext.Claims
                          join p in _claimsContext.Persons on c.PersonId equals p.Id
                          join s in _claimsContext.ClaimStatuses on c.StatusId equals s.Id
                          join cl in _claimsContext.Classes on c.ClassId equals cl.Id
                          join co in _claimsContext.Courses on c.CourseId equals co.Id
                          select new ClaimViewModel
                          {
                              Id = c.Id,
                              DateClaimed = c.DateClaimed,
                              ClassName = cl.ClassName,
                              CourseName = co.Name,
                              CourseId = c.CourseId,
                              CourseCode = co.CourseCode,
                              LectureFirstName = p.FirstName,
                              LectureLastName = p.LastName,
                              Rate = c.Rate,
                              TotalFee = c.TotalFee,
                              TotalHours = c.TotalHours,
                              ClassId = c.ClassId,
                              StatusId = c.StatusId,
                              PersonId = c.PersonId,
                              AdditionalNotes = c.AdditionalNotes,
                              DocumentPath = c.DocumentPath
                              
                          }
                ).OrderByDescending(x => x.DateClaimed).ToList();
            return claims;
        }

        public async Task<int> AddClaimAsync(Claim claim) // Updated to be asynchronous
        {
            var rate = await _claimsContext.Rates.FirstOrDefaultAsync(); // Use async method
            if (rate != null)
            {
                claim.Rate = rate.HourlyRate;
                double totalFee = claim.TotalHours * claim.Rate;
                claim.TotalFee = totalFee;
                await _claimsContext.Claims.AddAsync(claim); // Use async method for adding the claim
                await _claimsContext.SaveChangesAsync(); // Use async method for saving changes
                return claim.Id;
            }
            return 0;
        }

        public int UpdateClaim(Claim claim)
        {
            var _claim = _claimsContext.Claims.FirstOrDefault(x => x.Id == claim.Id);
            if (_claim != null)
            {
                double totalFee = claim.TotalHours * claim.Rate;
                _claim.TotalFee = totalFee;
                _claim.DateClaimed = claim.DateClaimed;
                _claim.ClassId = claim.ClassId;
                _claim.StatusId = claim.StatusId;
                _claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<ClaimViewModel> GetAllClaimsForLecturer(string username)
        {
            var person = _claimsContext.Persons.FirstOrDefault(p => p.EmailAddress == username);
            if (person == null)
            {
                return new List<ClaimViewModel>();
            }
            // search on the db and return the user claims
            var claims = (from c in _claimsContext.Claims
                          join p in _claimsContext.Persons on c.PersonId equals p.Id
                          join s in _claimsContext.ClaimStatuses on c.StatusId equals s.Id
                          join cl in _claimsContext.Classes on c.ClassId equals cl.Id
                          join co in _claimsContext.Courses on c.CourseId equals co.Id
                          where c.PersonId == person.Id
                          select new ClaimViewModel
                          {
                              Id = c.Id,
                              DateClaimed = c.DateClaimed,
                              ClassName = cl.ClassName,
                              CourseName = co.Name,
                              CourseId = c.CourseId,
                              CourseCode = co.CourseCode,
                              LectureFirstName = p.FirstName,
                              LectureLastName = p.LastName,
                              Rate = c.Rate,
                              TotalFee = c.TotalFee,
                              TotalHours = c.TotalHours,
                              ClassId = c.ClassId,
                              StatusId = c.StatusId,
                              PersonId = c.PersonId,
                              AdditionalNotes = c.AdditionalNotes,
                              DocumentPath = c.DocumentPath,
                              Status = s.Status
                          }
                ).OrderByDescending(x => x.DateClaimed).ToList();

            return claims.OrderByDescending(x => x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }



        // Get all pending claims
        public List<ClaimViewModel> GetPendingClaims()
        {


            var pendingClaims = (from c in _claimsContext.Claims
                                 join p in _claimsContext.Persons on c.PersonId equals p.Id
                                 join s in _claimsContext.ClaimStatuses on c.StatusId equals s.Id
                                 join cl in _claimsContext.Classes on c.ClassId equals cl.Id
                                 join co in _claimsContext.Courses on c.CourseId equals co.Id
                                 where c.StatusId == 1 //StatusId = 1 is 'Pending'
                                 select new ClaimViewModel
                                 {
                                     Id = c.Id,
                                     DateClaimed = c.DateClaimed,
                                     ClassName = cl.ClassName,
                                     CourseName = co.Name,
                                     CourseId = c.CourseId,
                                     CourseCode = co.CourseCode,
                                     LectureFirstName = p.FirstName,
                                     LectureLastName = p.LastName,
                                     Rate = c.Rate,
                                     TotalFee = c.TotalFee,
                                     TotalHours = c.TotalHours,
                                     ClassId = c.ClassId,
                                     StatusId = c.StatusId,
                                     PersonId = c.PersonId,
                                     AdditionalNotes = c.AdditionalNotes,
                                     DocumentPath = c.DocumentPath
                                 }
                ).OrderByDescending(x => x.DateClaimed).ToList();
            return pendingClaims;
        }

        // Approve a claim by setting StatusId to 'Approved'
        public void ApproveClaim(int claimId)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.StatusId = 2; //StatusId = 2 is 'Approved'
                _claimsContext.SaveChanges();
            }
        }

        // Update claim status 
        public void UpdateClaimStatus(int claimId, string status, string rejectReason)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                if (status == "Rejected")
                {
                    claim.StatusId = 3; //Rejected
                    claim.RejectReason = rejectReason;  //store reason
                }
                else if (status == "Approved")
                {
                    claim.StatusId= 2;  //approved
                }
                else
                {
                    claim.StatusId= 1;  //pending
                }
                _claimsContext.SaveChanges();
            }
        }
        //reject method
        public void RejectClaim(int claimId, string rejectReason)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.StatusId = 3; //Rejected status
                claim.RejectReason = rejectReason; //store the rejection reason
                _claimsContext.SaveChanges(); 
            }
        }
        

    }
}

//resources-https://github.com/IIEMSA/poe-part-2-dmaramba
