using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POE_Claim_System.Services
{
    public class ClaimService
    {
        private readonly ClaimsContext _context;

        public ClaimService(ClaimsContext claimsContext)
        {
            _context = claimsContext;
        }

        public Person GetUserByUsername(string username)
        {
            return _context.Persons.FirstOrDefault(p => p.Username == username);
        }

        public void AddPerson(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
        }

        public bool ValidateUser(string username, string password, string role)
        {
            try
            {
                return _context.Persons.Any(p => p.Username == username && p.Password == password && p.Role == role);
            }
            catch (Exception ex)
            {
                // Log exception details
                Console.WriteLine(ex.Message); // Replace with proper logging
                return false;
            }
        }



        public async Task AddClaimAsync(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        public List<ClaimView> GetPendingClaims()
        {
            return _context.Claims
                .Where(c => c.ClaimStatus.Status == "pending")
                .Select(c => new ClaimView
                {
                    Id = c.Id,
                    PersonId = c.PersonId,
                    LecturerName = c.Person.FirstName,
                    LecturerSurname = c.Person.LastName,
                    CourseId = c.CourseId,
                    TotalHours = c.TotalHours,
                    Rate = c.Rate,
                    TotalFee = c.TotalFee,
                    DateClaimed = c.DateClaimed,
                    StatusId = c.StatusId,
                    Status = c.ClaimStatus.Status,
                    AdditionalNotes = c.AdditionalNotes
                })
                .ToList();
        }

        public List<Claim> GetAllClaimsForUser(string username)
        {
            return _context.Claims.Where(c => c.Username == username).ToList();
        }


        public void UpdateClaimStatus(int claimId, string status)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null)
            {
                claim.ClaimStatus.Status = status;
                _context.SaveChanges();
            }
        }

        public void ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "approved";
                _context.SaveChanges();
            }
        }
    }
}


/*
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
           
        public int AddNewClaim(Claim claim)
        {
            //logic to add to claim to db

            //how to get rate defined by lecturer to databse
            var rate = claimsContext.Rates.FirstOrDefault(x => x.PersonId == claim.PersonId);
            if (rate != null)
            {
                //attach claim
                claim.Rate = rate.HourlyRate;

                //exception if record not found
                double totalFee = claim.TotalHours * claim.Rate;
                claim.TotalFee = totalFee;
                claimsContext.Claims.Add(claim);
                claimsContext.SaveChanges();
                return claim.Id;
            }
            return 0;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to uodate claim to db
            //get record
            var _claim = claimsContext.Claims.FIrstOrDefault(x => x.Id == claim.Id);
            if(_claim != null)
            {
                double totalFee = claim.TotalHours * claim.Rate;
                _claim.TotalFee = totalFee;
                _claim.DateClaimed = claim.DateClaimed;
                _claim.ClassId = claim.ClassId;
                _claim.StatusId = claim.StatusId;
                //add more fields to update
                claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<Claim> GetAllClaimsForUser(int personId)
        {
            //search on db and return user claims
            //look for claim
            //create select statement
            var claims = claimsContext.Claims.Where(x => x.PersonId == personId).ToList();

            //return new List<ClaimService>();
            return claims.OrderByDescending(x =>  x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }

    }
}
*/
