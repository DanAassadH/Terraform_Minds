using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;

namespace TerraformMinds.Controllers
{
    public class StudentController : Controller
    {
        /* ------------------------------------------Actions -----------------------------------------------------*/
        [Authorize(Roles = "Student")]
        public IActionResult StudentDashboard()
        {
          return View();

        }

        /// <summary>
        /// This is the Action for course List page for individual student , it shows that how many courses that student has
        /// </summary>
        /// <param name="id"></param>
        /// <returns> View() </returns>

        [Authorize(Roles = "Student")]
        public IActionResult CourseList() 
        {
            try
            {
                ViewBag.StudentsCourses = GetCourseByStudentID();
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
        /// <summary>
        /// This function grabs course List for individual instructor
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of courses</returns>
        public List<Course> GetCourseByStudentID()
        {
            ValidationException exception = new ValidationException();
            List<Course> studentsCourses = null;

            if (User.Identity.Name != null)
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    // Sql Query : 
                    // SELECT a.CourseName, a.Subject , a.CourseDescription FROM `Course` a , `student` b WHERE a.ID=b.CourseID AND b.UserID = 6(id)
                       studentsCourses = context.Courses.Where(x => x.Students.Any(y => y.CourseID == x.ID)).Where(x => x.Students.Any(y => y.UserID == int.Parse(User.Identity.Name))).ToList();

                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Something went wrong please Logout and try again"));
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }
            return studentsCourses;
        }

    }
}
