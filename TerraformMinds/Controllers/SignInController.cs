using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TerraformMinds.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using TerraformMinds.Models.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
                                new Claim(ClaimTypes.Name, EMail),
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
                                new Claim(ClaimTypes.Name, EMail),
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


                        // return RedirectToAction("Index" , "Home");
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

        /* ----------------------------------------------- Signout ----------------------------------------*/

       // [HttpPost]
        public IActionResult Signout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Privacy", "Home");
        }


        /* ----------------------------------------------- Data ----------------------------------------*/

        /// <summary>
        /// Validates user given email and password with the ones in database
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Password"></param>
        /// <returns> validUser as the user credentials or null </returns>

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


        /// <summary>
        /// ///////// Convert password to a Hash of random Letters Using a known salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns><param string="Hashed Password"></param></returns>

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
