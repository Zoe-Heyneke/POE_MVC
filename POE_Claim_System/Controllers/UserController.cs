﻿using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult SelectRole(string username, string password, string role)
        {
            // You might want to add logic here to validate username and password

            if (role == "Lecturer")
            {
                return RedirectToAction("Index", "Lecturer");
            }
            else if (role == "CM") // Assuming CM stands for Coordinator and Manager
            {
                return RedirectToAction("Index", "CM");
            }

            // Optional: handle case where role is not recognized
            return RedirectToAction("Error");
        }
    }
}