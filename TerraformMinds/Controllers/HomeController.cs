﻿using Microsoft.AspNetCore.Mvc;
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

        // UN-COMMENT ONCE WE HAVE OVER 10 Instructors, Students and Courses.
        //public IActionResult Index()
        //{
        //    ViewBag.CourseCount = RoundDown(CourseCount().Count());
        //    ViewBag.InstructorCount = RoundDown(InstructorCount().Count());
        //    ViewBag.StudentCount = RoundDown(StudentCount().Count());

        //    return View();
        //}

        public IActionResult Index()
        {
            ViewBag.CourseCount = CourseCount().Count();
            ViewBag.InstructorCount = InstructorCount().Count();
            ViewBag.StudentCount = StudentCount().Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public IActionResult HomeCards()
        //{


        //    return View();
        //}

        public List<Course> CourseCount()
        {
            List<Course> coursesCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                coursesCount = context.Courses.ToList();
            }

            return coursesCount;
        }

        public List<User> InstructorCount()
        {
            List<User> instructorCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                instructorCount = context.Users.Where(x => x.Role == 2).ToList();
            }

            return instructorCount;
        }

        public List<Student> StudentCount()
        {
            List<Student> studentCount;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                studentCount = context.Students.ToList();
            }

            return studentCount;
        }

        public int RoundDown(int toRound)
        {
            if(toRound % 10 == 0)
            {
                return toRound;
            }
            else
            {
                return toRound - toRound % 10;
            }
        }
    }
}
