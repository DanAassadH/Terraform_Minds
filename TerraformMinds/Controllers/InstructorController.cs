using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;

namespace TerraformMinds.Controllers
{
    public class InstructorController : Controller
    {
        /* ------------------------------------------Actions -----------------------------------------------------*/
        [Authorize(Roles = "Instructor")]
        public IActionResult InstructorDashboard()
        {
            // return RedirectToAction("CourseList");
            return View();

        }

        [Authorize(Roles = "Instructor")]
        public IActionResult CourseList(string id)
        {
            try
            {
                ViewBag.InstructorsCourses = GetCourseByInstructorID(id);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }

            return View();
        }

        /* ------------------------------------------Data -----------------------------------------------------*/

        public  List<Course> GetCourseByInstructorID(string id)
        {
            ValidationException exception = new ValidationException();
            List<Course> instructorsCourses  = null;

            // Prevent user from accessing any other user's record
            if (User.Identity.Name == id)
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {

                    instructorsCourses = context.Courses.Where(x => x.UserID == int.Parse(id)).ToList();
                }              
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("If You Are experiencing trouble accessing your courses , Go back to main Instructor Dsahboard and select course again"));
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }
            return instructorsCourses;
        }
    }
}
