//using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Services;
using POE_Claim_System.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO; // Add for working with file streams
//using Microsoft.AspNetCore.Http; // For IFormFile interface

namespace POE_Claim_System.Services
{
    public class ClaimService
    {
        private readonly ClaimsContext _claimsContext; // Injected service

        // Constructor injection
        public ClaimService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext; // Use dependency injection for the context
        }

        public ClaimService()
        {
        }

        // Get all pending claims (you need to define what "pending" means, assuming a status ID of 1 represents pending claims)
        public List<Claim> GetPendingClaims()
        {
            return _claimsContext.Claims
                .Where(x => x.StatusId == 1) // Assuming StatusId of 1 means "pending"
                .ToList();
        }

        // Update claim status
        public void UpdateClaimStatus(int claimId, string status)
        {
            var claim = _claimsContext.Claims.FirstOrDefault(x => x.Id == claimId);
            if (claim != null)
            {
                if (status == "approved")
                {
                    claim.StatusId = 2; // Assuming StatusId of 2 means "approved"
                }
                else if (status == "rejected")
                {
                    claim.StatusId = 3; // Assuming StatusId of 3 means "rejected"
                }
                _claimsContext.SaveChanges();
            }
        }

        // Other methods like AddNewClaim, GetAllClaimsForUser, and UpdateClaim can remain as they are
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
            return 1;
        }

        public List<Claim> GetAllClaimsForUser(int personId)
        {
            return _claimsContext.Claims
                .Where(x => x.PersonId == personId)
                .OrderByDescending(x => x.DateClaimed)
                .ThenBy(x => x.StatusId)
                .ToList();
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

        // Get claim by ID
        public Claim GetClaimById(int claimId)
        {
            var claim = _claimsContext.Claims.Include(c => c.ClaimStatus).FirstOrDefault(x => x.Id == claimId);
            return claim; // Returns null if not found
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
