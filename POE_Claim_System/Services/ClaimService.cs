using POE_Claim_System.Models;

namespace POE_Claim_System.Services
{
    public class ClaimService
    {
        private readonly ClaimsContext _claimsContext;

        public ClaimService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext; // Injected via DI
        }

        public int AddNewClaim(Claim claim)
        {
            var rate = _claimsContext.Rates.FirstOrDefault(x => x.PersonId == claim.PersonId);
            if (rate != null)
            {
                claim.Rate = rate.HourlyRate;
                double totalFee = claim.TotalHours * claim.Rate;
                claim.TotalFee = totalFee;
                _claimsContext.Claims.Add(claim);
                _claimsContext.SaveChanges();
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
            var person = _claimsContext.Persons.FirstOrDefault(p => p.Username == username); // Assuming a Persons table has Username field
            if (person == null)
            {
                return new List<Claim>(); // Return an empty list if no person is found
            }

            var claims = _claimsContext.Claims.Where(x => x.PersonId == person.Id).ToList();
            return claims.OrderByDescending(x => x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }


        // New Methods

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
