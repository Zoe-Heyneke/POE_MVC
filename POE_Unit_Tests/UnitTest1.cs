using POE_Claim_System.Services;

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
    }
}