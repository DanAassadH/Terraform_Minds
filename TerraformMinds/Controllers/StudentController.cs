using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult CourseList(string id)
        {
/*            try
            {
                // Calling function from Instructor controller to display the list of courses by given id
                ViewBag.StudentsCourses = InstructorController.GetCourseByUserID(id);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }*/

            return View();
        }
    }
}
