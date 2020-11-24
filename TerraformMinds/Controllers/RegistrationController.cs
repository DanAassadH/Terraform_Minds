using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;

namespace TerraformMinds.Controllers
{
    public class RegistrationController : Controller
    {
        /* ----------------------------------------------- Actions ----------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("RegisterUser");
           
        }

        public IActionResult RegisterUser(string firstName, string lastName, string email, string password, string role)
        {
            /* If else to check if register new user button clicked*/
            if (Request.Method == "POST")
            {
                Register(firstName, lastName, email, password, role);
            }
                

            return View();
        }


        /* ----------------------------------------------- Data ------------------------------------------*/

        public void Register(string firstName, string lastName , string email, string password, string role)
        {
            ValidationException exception = new ValidationException();

            // Trim the values 
            firstName = firstName != null ? firstName.Trim() : null;
            lastName = lastName != null ? lastName.Trim() : null;
            email = email != null ? email.Trim() : null;
            password = password != null ? password.Trim() : null;
            role = role != null ? role.Trim() : null;



            using (LearningManagementContext context = new LearningManagementContext())
            {

                context.Users.Add(new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EMail = email,
                    Password = password,
                    Role = int.Parse(role),
                    JoinDate = DateTime.Now
                }) ;
                context.SaveChanges();

            }



        }
   }
}
