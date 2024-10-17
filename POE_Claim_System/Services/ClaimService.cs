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

        public ClaimService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext; // Injected via DI
            _claimsContext.Database.EnsureCreated();
        }

        public List<Claim> GetAllClaims()
        {
            return _claimsContext.Claims
                .Include(c => c.Person)   
                .Include(c => c.Course)   
                .OrderByDescending(c => c.DateClaimed)
                .ToList();
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

        public List<Claim> GetAllClaimsForUser(string username)
        {
            var person = _claimsContext.Persons.FirstOrDefault(p => p.EmailAddress == username); 
            if (person == null)
            {
                return new List<Claim>(); 
            }

            var claims = _claimsContext.Claims.Where(x => x.PersonId == person.Id).ToList();
            return claims.OrderByDescending(x => x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }

        

        // Get all pending claims
        public List<Claim> GetPendingClaims()
        {
            var pendingClaims = _claimsContext.Claims
                                             .Where(c => c.StatusId == 1) // Assuming StatusId = 1 is 'Pending'
                                             .OrderByDescending(c => c.DateClaimed)
                                             .ToList();
            return pendingClaims;
        }

        // Approve a claim by setting StatusId to 'Approved'
        public void ApproveClaim(int claimId)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.StatusId = 2; // Assuming StatusId = 2 is 'Approved'
                _claimsContext.SaveChanges();
            }
        }

        // Update claim status to 'Rejected' or any other status
        public void UpdateClaimStatus(int claimId, string status)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                if (status == "Rejected")
                {
                    claim.StatusId = 3; // Assuming StatusId = 3 is 'Rejected'
                }
                _claimsContext.SaveChanges();
            }
        }
    }
}
