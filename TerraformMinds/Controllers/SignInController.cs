using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TerraformMinds.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using TerraformMinds.Models.Exceptions;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;

namespace TerraformMinds.Controllers
{
    public class SignInController : Controller
    {
        /* ----------------------------------------------- Actions ----------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("SignIn");

        }

        /// <summary>
        /// Action to sign in already existing user and send them to appropriate dashboard according to their role
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public IActionResult SignIn(string EMail, string Password)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            User user;
            if (Request.Method == "POST")
            {

                try
                {
                    user = ValidateSignIn(EMail, Password);
                    if (user != null)
                    {

                        //Create the identity for the user  

                        if (user.Role == 3)
                        {
                            identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, user.ID.ToString()),
                                new Claim(ClaimTypes.Role, "Student") }, CookieAuthenticationDefaults.AuthenticationScheme);

                            isAuthenticated = true;

                            if (isAuthenticated)
                            {
                                var principal = new ClaimsPrincipal(identity);
                                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                // return RedirectToAction("Student", "Rolecheck");
                                return RedirectToAction("StudentDashboard", "Student");
                            }
                        }

                        if (user.Role == 2)
                        {
                            identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, user.ID.ToString()),
                                new Claim(ClaimTypes.Role, "Instructor"),
                            }, CookieAuthenticationDefaults.AuthenticationScheme);

                            isAuthenticated = true;

                            if (isAuthenticated)
                            {
                                var principal = new ClaimsPrincipal(identity);
                                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                // return RedirectToAction("Instructor", "Rolecheck");
                                return RedirectToAction("InstructorDashboard", "Instructor");
                            }
                        }

                        if (user.Role == 1)
                        {
                            identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, user.ID.ToString()),
                                new Claim(ClaimTypes.Role, "Administrator") }, CookieAuthenticationDefaults.AuthenticationScheme);

                            isAuthenticated = true;

                            if (isAuthenticated)
                            {
                                var principal = new ClaimsPrincipal(identity);
                                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                //  return RedirectToAction("Admin", "Rolecheck");
                                return RedirectToAction("AdministratorDashboard", "Administrator");
                            }
                        }

                    }
                    else
                    {
                        ViewBag.Message = "The email or password you've entered is incorrect. Please try again";
                    }

                }
                catch (ValidationException e)
                {
                    ViewBag.Message = "Something went wrong";
                    ViewBag.Exception = e;
                    ViewBag.Error = true;
                }


            }

            return View();
        }

        /* ----------------------------------------------- Signout ----------------------------------------*/
        /// <summary>
        /// Action to signout user and redirect them to home page
        /// </summary>
        /// <returns></returns>
       public IActionResult Signout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        /* ----------------------------------------------- Data ----------------------------------------*/

        /// <summary>
        /// Validates user given email and password with the ones in database
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Password"></param>
        /// <returns> valid User or null </returns>

        [ValidateAntiForgeryToken]
        public User ValidateSignIn(string EMail, string Password)
        {
            ValidationException exception = new ValidationException();
            // Trim the values 
            Password = Password?.Trim();
            EMail = EMail?.Trim();
      
            string password;
            User validUser = null;
            bool flag = false;


            /// Validation for email
            if (string.IsNullOrWhiteSpace(EMail))
            {
                exception.ValidationExceptions.Add(new Exception("EMail Required"));
                flag = true;
            }
            else if (EMail.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("Maximum Character limit for Email is 50"));
                flag = true;
            }
            else if (!Regex.IsMatch(EMail, @"^[\w-!$*%^\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                exception.ValidationExceptions.Add(new Exception("Incorrect Email Address  "));
                flag = true;
            }

            // Validation for password
            if (string.IsNullOrWhiteSpace(Password))
            {
                exception.ValidationExceptions.Add(new Exception("Password Required"));
                flag = true;

            }
            else if(Password.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("Maximum Character limit for password is 50"));
                flag = true;
            }


            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            if (flag==false)
            {
                // Add Password Hash here
                password = HashAndSaltPassowrd(Password, EMail.ToUpper());
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    // Validating User
                    validUser = context.Users.Where(x => x.EMail.ToUpper().Equals(EMail.ToUpper()) && x.Password.Equals(password)).SingleOrDefault();
                }
               
            }
            return validUser;
        }


        /// <summary>
        /// Convert password to a Hash of random characters Using a known salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>Hashed Password as a string</returns>

        public static string HashAndSaltPassowrd(string password, string email)
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
