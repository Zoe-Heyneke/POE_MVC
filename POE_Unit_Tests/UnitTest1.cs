
using System.Collections.Generic;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POE_Unit_Tests
{
    public class UnitTest1
    {
        /*example
        [Fact]
        
        public void Test1()
        {
            TestingService service = new TestingService();
            var result = service.Sum(2, 6);

            Assert.Equal(8, result);    //correct
        }
        */

        //create inmemory for temoporary test data
        public class DocumentServiceTests
        {
            private readonly ClaimsContext _context;
            private readonly DocumentService _documentService;

            public DocumentServiceTests()
            {
                //inmemory db for testing with temp values
                var options = new DbContextOptionsBuilder<ClaimsContext>()
                    .UseInMemoryDatabase(databaseName: "TestDb")
                    .Options;

                _context = new ClaimsContext(options);
                _documentService = new DocumentService(_context);
            }

            [Fact]
            public void AddClaimDocument_ShouldAddDocument()
            {
                // Arrange
                var document = new Document { Id = 1, DocumentName = "test1.pdf" };

                // Act
                var result = _documentService.AddClaimDocument(document);

                //Assert (actual value that is true)
                Assert.Equal(1, result); //give correct number of id
                Assert.Single(_context.Documents); //add one document
            }
        }
    }
}