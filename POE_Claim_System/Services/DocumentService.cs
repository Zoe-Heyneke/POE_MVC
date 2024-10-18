using POE_Claim_System.Models;

namespace POE_Claim_System.Services
{
    public class DocumentService
    {
        ClaimsContext _claimsContext;
        public DocumentService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext;
            _claimsContext.Database.EnsureCreated();
        }

        //add new document
        public int AddClaimDocument(Document claimDocument)
        {
            _claimsContext.Documents.Add(claimDocument);
            _claimsContext.SaveChanges();
            return claimDocument.Id;
        }

        public void DeleteClaimDocument(int claimDocumentId)
        {
            var claimDocument = _claimsContext.Documents.FirstOrDefault(x => x.Id == claimDocumentId);
            if (claimDocument != null)
            {
                _claimsContext.Documents.Remove(claimDocument);
                _claimsContext.SaveChanges();
            }
        }
        //get documents for a claim
        public List<Document> GetClaimDocuments(int claimId)
        {
            var claimDocuments = _claimsContext.Documents.Where(x => x.ClaimId == claimId).ToList();
            return claimDocuments;
        }
    }
}
