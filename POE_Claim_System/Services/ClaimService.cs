using Microsoft.AspNetCore.Mvc.ViewFeatures;
using POE_Claim_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClaimService
{
    private readonly ClaimContext claimsContext;

    // Constructor to inject the context using Dependency Injection
    public ClaimService(ClaimContext _context)
    {
        claimsContext = _context;
    }

    // Get all claims for a specific user by PersonId
    public List<Claim> GetAllClaimsForUser(int personId)
    {
        // Retrieves all claims for the given personId
        return claimsContext.Claims.Where(x => x.PersonId == personId).ToList();
    }

    // Method to add a new claim and calculate its total fee
    public int AddNewClaim(Claim claim)
    {
        // Retrieve the rate associated with the person (PersonId)
        var rate = claimsContext.Rates.FirstOrDefault(x => x.PersonId == claim.PersonId);

        if (rate != null)
        {
            // If a rate exists, calculate the total fee based on hours worked and rate
            claim.Rate = rate.HourlyRate;
            claim.TotalFee = claim.TotalHours * claim.Rate;

            // Add the claim to the database and save changes
            claimsContext.Claims.Add(claim);
            claimsContext.SaveChanges();

            // Return the ID of the newly added claim
            return claim.Id;
        }

        // If no rate is found, return 0 or handle it appropriately
        return 0;
    }

    // Method to update an existing claim
    public bool DeleteClaim(int claimId)
    {
        // Retrieve the claim to delete
        var claim = claimsContext.Claims.FirstOrDefault(x => x.Id == claimId);

        if (claim != null)
        {
            // Remove the claim and save changes
            claimsContext.Claims.Remove(claim);
            claimsContext.SaveChanges();
            return true;
        }

        return false;
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
            return claims.OrderByDescending(x =>  x.DateClaimed).Thenby(x => x.StatusId).ToList();
        }

    }
}
*/
