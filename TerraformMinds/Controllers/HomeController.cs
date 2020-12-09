using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TerraformMinds.Models;

namespace TerraformMinds.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Counts the total amount of courses, instructors, and students.
        /// </summary>
        /// <returns>Displays number count rounded down to the nearest even number.</returns>
        public IActionResult Index()
        {
            ViewBag.CourseCount = RoundDown(CourseCount().Count());
            ViewBag.InstructorCount = RoundDown(InstructorCount().Count());
            ViewBag.StudentCount = RoundDown(StudentCount().Count());

            return View();
        }

        /// <summary>
        /// Loads to About view page
        /// </summary>
        /// <returns>About View</returns>
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Adds all courses found in database into a list of courses
        /// </summary>
        /// <returns>A list of courses</returns>
        public List<Course> CourseCount()
        {
            List<Course> coursesCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                coursesCount = context.Courses.ToList();
            }

            return coursesCount;
        }

        /// <summary>
        /// Adds all instructors found in database into a list of instructors
        /// </summary>
        /// <returns>A list of instructors</returns>
        public List<User> InstructorCount()
        {
            List<User> instructorCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                instructorCount = context.Users.Where(x => x.Role == 2).ToList();
            }

            return instructorCount;
        }

        /// <summary>
        /// Adds all students found in database into a list of students
        /// </summary>
        /// <returns>A list of students</returns>
        public List<User> StudentCount()
        {
            List<User> studentCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                studentCount = context.Users.Where(x => x.Role == 3).ToList();
            }

            return studentCount;
        }

        /// <summary>
        /// Checks if value is even, if not round down to nearest even number
        /// </summary>
        /// <param name="toRound"></param>
        /// <returns>An even number</returns>
        public int RoundDown(int toRound)
        {
            if (toRound % 2 == 0)
            {
                return toRound;
            }
            else
            {
                return toRound - toRound % 2;
            }
        }
    }
}
