using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;
using Microsoft.AspNetCore.Authorization;
using TerraformMinds.Models.Exceptions;

namespace TerraformMinds.Controllers
{
    public class SharedFunctionsController : Controller
    {
        /* This file contains list of functions that are resued in more than one controller*/

        /// <summary>
        /// Function to get the information about user using their Signed in user ID  
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>All user information</returns>
        public static User GetUserNameBySignInID(string userID)
        {
            ValidationException exception = new ValidationException();
            User user= null;

            if (int.TryParse(userID, out int parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    user = context.Users.Where(x => x.ID == parsedId).SingleOrDefault();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("We encountered a problem , Please logout and try again "));
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return user;
        }
    }
}
