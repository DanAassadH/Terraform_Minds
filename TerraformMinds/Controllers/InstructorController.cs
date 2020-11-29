﻿using System;
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

        /// <summary>
        /// This is the Action for course List page for individual instructor , it shows that how many courses that instructor has
        /// </summary>
        /// <param name="id"></param>
        /// <returns> View() </returns>

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

        /// <summary>
        /// Action for Course details page this page gives detail about student and assignments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult CourseDetail(string id)
        {
            try
            {
                ViewBag.SingleCourseDetail = GetCourseDetailsByID(id);
                if(ViewBag.SingleCourseDetail != null)
                {
                    // Get students enrolled in this course
                    ViewBag.StudentsForCourse = GetStudentsByCourseID(id);
                }
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
        public List<Course> GetCourseByInstructorID(string id)
        {
            ValidationException exception = new ValidationException();
            List<Course> instructorsCourses = null;

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



        /// <summary>
        /// Function to grab details of the signle course whose ID is provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns> details of a single course</returns>

        public Course GetCourseDetailsByID(string id)
        {
            ValidationException exception = new ValidationException();
            Course courseDetail = null;
            int parsedId;
            int userId = int.Parse(User.Identity.Name);

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            // check if id is non integer
            if (id == null) 
            {
                exception.ValidationExceptions.Add(new Exception("No ID Provided, Go back to main Instructor Dsahboard and select course again"));
            }
            else
            {
                if (int.TryParse(id, out parsedId))
                {
                    using (LearningManagementContext context = new LearningManagementContext())
                    {

                        courseDetail = context.Courses.Where(x => x.ID == parsedId && x.UserID == userId).SingleOrDefault();
                    }
                }
                else
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid ID , Go back to main Instructor Dsahboard and select course again"));
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return courseDetail;
        }


        public List<User> GetStudentsByCourseID(string id)
        {
            ValidationException exception = new ValidationException();
            List<User> studentNames = null;
            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("No student ID Provided, Go back to main Instructor Dsahboard and select course again"));
            }
            else
            {
                if (int.TryParse(id, out parsedId))
                {
                    using (LearningManagementContext context = new LearningManagementContext())
                    {
                        // sql query :
                        //SELECT a.FirstName, a.LastName FROM `user` a , student b WHERE a.ID=b.UserID AND CourseID = -2(id)
                        //  studentNames = context.Courses.Where(x => x.ID == parsedId && x.UserID == userId).SingleOrDefault();

                        studentNames = context.Users.Where(x => x.Role == 3).ToList();
                    }
                }
                else
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid student ID , Go back to main Instructor Dsahboard and select course again"));
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return studentNames;

        }
    }
}
