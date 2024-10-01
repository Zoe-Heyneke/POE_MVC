namespace POE_Claim_System.Services
{
    public class ClaimService
    {
        public ClaimService() { }

        public int AddNewClaim(ClaimService claim)
        {
            //logic to add to claim to db

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
