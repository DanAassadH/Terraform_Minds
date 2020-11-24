﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

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
                    Password = HashAndSaltPassowrd(password,email),
                    Role = int.Parse(role),
                    JoinDate = DateTime.Now
                }) ;
                context.SaveChanges();

            }



        }

        public string HashAndSaltPassowrd(string password , string email)
        {

            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0
            // Generate a SALT
            // Convert string to a byte array Using a known salt insted of random . for proper security there should be a SALT field in database, and every password should have a different SALT but for this application we are using a combination of email+password as a SALT 

            byte[] salt = Encoding.ASCII.GetBytes(email + password);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


            return hashed;
        }
   }
}
