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
            ClaimsContext.Claims.Add(claim);
            ClaimsContext.SaveChanges();
            return claim.Id;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to uodate claim to db

            return claim.Id;
        }

        public List<ClaimService> GetAllClaimsForUser(int personId)
        {
            //search on db and return user claims

            return new List<ClaimService>();
        }

    }
}
