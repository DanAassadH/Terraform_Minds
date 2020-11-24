using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;

namespace TerraformMinds.Controllers
{
    public class RegistrationController : Controller
    {
        /* -------------------------------------------- Actions -------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("RegisterUser");
           
        }

        public IActionResult RegisterUser()
        {
            /* If else to check if register new user button clicked*/
            return View();
        }


        /* -------------------------------------------- Data -------------------------------------*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Register(string firstName, string lastName , string email, string )
        {
            
        }
   }
}
