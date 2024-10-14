using Microsoft.AspNetCore.Mvc.ViewFeatures;
using POE_Claim_System.Models;

namespace POE_Claim_System.Services
{
    //declare context to be accessible
    ClaimsContext claimsContext;
    public class ClaimService()
    {
        public ClaimService()
        {
            claimsContext = new ClaimsContext();
        }
           
        public int AddNewClaim(ClaimService claim)
        {
            //logic to add to claim to db
            double totalFee = claim
            claimsContext.Claims.Add(claim);
            claimsContext.SaveChanges();
            return claim.Id;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to uodate claim to db
            //get record
            var _claim = claimsContext.Claims.FIrstOrDefault(x => x.Id == claim.Id);
            if(_claim != null)
            {
                _claim.DateClaimed = claim.DateClaimed;
                _claim.ClassId = claim.ClassId;
                _claim.StatusId = claim.StatusId;
                //add more fields to update
                claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<ClaimService> GetAllClaimsForUser(int personId)
        {
            //search on db and return user claims

            return new List<ClaimService>();
        }

    }
}
