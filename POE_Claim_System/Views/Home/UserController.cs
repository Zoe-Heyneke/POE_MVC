using Microsoft.AspNetCore.Mvc;
using POE_Claim_System.Models;

namespace POE_Claim_System.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult SelectRole(Person model)
        {
            // You might want to add logic here to validate username and password

            if (model.Role == "Lecturer")
            {
                return RedirectToAction("Index", "Lecturer");
            }
            else if (model.Role == "CM")
            {
                return RedirectToAction("Index", "Coordinator");
            }

            // Optionally return to the Index page with an error message if role is not valid
            return RedirectToAction("Index", "Home");
        }
    }
}
