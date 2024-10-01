using POE_Claim_System.Services;

namespace POE_Unit_Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            TestingService service = new TestingService();
            var result = service.Sum(2, 6);

            Assert.Equal(8, result);    //correct
        }
    }
}