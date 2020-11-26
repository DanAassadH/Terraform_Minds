using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TerraformMinds.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using TerraformMinds.Models.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TerraformMinds.Controllers
{
    public class SignInController : Controller
    {
        /* ----------------------------------------------- Actions ----------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("SignIn");

        }

        public IActionResult SignIn(string EMail, string Password)
        {
            User user;
            if (Request.Method == "POST")
            {

                try
                {
                    user = ValidateSignIn(EMail, Password);
                    if (user != null)
                    {

                        ViewBag.Message = $"Yaya Successfully Logged In User!";
                  //      FormsAuthentication.SetAuthCookie(Model.Email, false);
                  // Add switch or if else for user role and send to appropriate view 
                        return RedirectToAction("Index" , "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Email and Password dont match";
                        ViewBag.Error = true;
                    }

                }
                catch (ValidationException e)
                {
                    ViewBag.Message = "Something went wrong";
                    // ViewBag.Exception = e;
                    ViewBag.Error = true;
                }


            }

            return View();
        }


        [ValidateAntiForgeryToken]
        public User ValidateSignIn(string EMail, string Password)
        {
            string password;
            User validUser;

            // Add Password Hash here
            password = HashAndSaltPassowrd(Password, EMail);

            using (LearningManagementContext context = new LearningManagementContext())
            {
                // Validating User
                validUser = context.Users.Where(x => x.EMail.Equals(EMail) && x.Password.Equals(password)).SingleOrDefault();
            }

            return validUser;
        }

        public string HashAndSaltPassowrd(string password, string email)
        {

            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0
            // Generate a SALT
            // Convert string to a byte array Using a known salt insted of random . for proper security there should be a SALT field in database, and every password should have a different SALT(generated through random function) , but for this application we are using a combination of email+password to generate SALT 

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
