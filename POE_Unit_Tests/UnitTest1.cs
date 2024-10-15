using POE_Claim_System.Services;
using POE_Claim_System.Models;

namespace POE_Unit_Tests
{
    public class UnitTest1
    {
        [Fact]
        /*
        public void Test1()
        {
            TestingService service = new TestingService();
            var result = service.Sum(2, 6);

            Assert.Equal(8, result);    //correct
        }
        */
        public void Test_AddNewClaim()
        {
            var claimService = new ClaimService();
            var claim = new Claim
            {
                TotalHours = 5,
                Rate = 50,
                PersonId = 1
            };

            var claimId = claimService.AddNewClaim(claim);

            Assert.True(claimId > 0);
        }

        [Fact]
        public void Test_ApproveClaim()
        {
            var claimService = new ClaimService();
            var claimId = 1;

            claimService.UpdateClaimStatus(claimId, "approved");

            var updatedClaim = claimService.GetClaimById(claimId);
            Assert.Equal("approved", updatedClaim.ClaimStatus.Status);
        }

        [Fact]
        public void Test_GetClaimById()
        {
            var claimService = new ClaimService(_mockClaimsContext);

            // Assume claimId 1 exists in the mocked database
            var claimId = 1;
            var claim = claimService.GetClaimById(claimId);

            Assert.NotNull(claim);  // Check that the claim exists
            Assert.Equal(claimId, claim.Id);  // Check that the correct claim is returned
        }

    }
}