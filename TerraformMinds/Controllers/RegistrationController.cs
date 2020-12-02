using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.RegularExpressions;

namespace TerraformMinds.Controllers
{
    public class RegistrationController : Controller
    {
        /* ----------------------------------------------- Actions ----------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("RegisterUser");
           
        }

        public IActionResult RegisterUser(string FirstName, string LastName, string EMail, string Password, string Role)
        {
            /* If else to check if register new user button clicked*/
            if (Request.Method == "POST")
            {
                try
                {
                    Register(FirstName, LastName, EMail, Password, Role);
                    ViewBag.Message = $"Successfully Registered User!";
                }
                catch (ValidationException e)
                {
                    ViewBag.Message = "There exist problem(s) with your submission, see below.";
                    ViewBag.Exception = e;
                    ViewBag.Error = true;
                }
                
            }
                
            return View();
        }


        /* ----------------------------------------------- Data ------------------------------------------*/
        /// <summary>
        /// Function To add Validated user credentials in User database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        public void Register(string firstName, string lastName , string email, string password, string role)
        {
            ValidationException exception = new ValidationException();

            // Trim the values 
            firstName = firstName?.Trim();
            lastName = lastName?.Trim();
            email = email?.Trim();
            password = password?.Trim();
            role = role?.Trim();
            bool flag = false;

            // Validation for First Name 
            if (string.IsNullOrWhiteSpace(firstName))
            {
                exception.ValidationExceptions.Add(new Exception("First Name Not Provided"));
                flag = true;
            }
            else if (firstName.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("First Name Cannot exceed 50 characters"));
                flag = true;
            }

            // Validation for Last Name 
            if (string.IsNullOrWhiteSpace(lastName))
            {
                exception.ValidationExceptions.Add(new Exception("Last Name Not Provided"));
                flag = true;
            }
            else if (lastName.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("Last Name Cannot exceed 50 characters"));
                flag = true;
            }

            // Validation for Email
            if (string.IsNullOrWhiteSpace(email))
            {
                exception.ValidationExceptions.Add(new Exception("Email Not Provided"));
                flag = true;
            }
            else if (email.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("Email Cannot exceed 50 characters"));
                flag = true;
            }
            else if (!Regex.IsMatch(email, @"^[\w-!$*%^\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                exception.ValidationExceptions.Add(new Exception("Incorrect Email Address  "));
                flag = true;
            }

            // Validation for Password
            if (string.IsNullOrWhiteSpace(password))
            {
                exception.ValidationExceptions.Add(new Exception("Password Not Provided"));
                flag = true;
            }
            else if (password.Length > 50)
            {
                exception.ValidationExceptions.Add(new Exception("Password Cannot exceed 50 characters"));
                flag = true;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                exception.ValidationExceptions.Add(new Exception("Please Select a Role"));
                flag = true;
            }


            if(flag==false)
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    // Checking for Email duplication
                    if ((context.Users.Where(x => (x.EMail.Trim().ToUpper()) == email.ToUpper()).Count()) > 0)
                    {
                        exception.ValidationExceptions.Add(new Exception("Email already exist, Try again with a new one"));
                        flag = true;
                    }

                    if(flag==false)
                    {
                        // Add Values in user Table if all validations are passed
                        context.Users.Add(new User()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            EMail = email.ToUpper(),
                            Password = SignInController.HashAndSaltPassowrd(password, email.ToUpper()),
                            Role = int.Parse(role),
                            JoinDate = DateTime.Now
                        });
                        context.SaveChanges();
                    }
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }
        }
   }
}
