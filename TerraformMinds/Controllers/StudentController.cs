using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Action for Course details page this page gives detail about Course and its assignments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student")]
        public IActionResult CourseDetail(string id) // passing CourseID
        {
            try
            {
                ViewBag.SingleCourseDetail = GetCourseDetailsByID(id);
                if (ViewBag.SingleCourseDetail != null)
                {
                    // Get Assignments for this course Calling this function from instructor controller
                    ViewBag.AssignmentsForCourse = InstructorController.GetAssignmentsByCourseID(id);
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
        public List<Course> GetCourseByStudentID()
        {
            ValidationException exception = new ValidationException();
            List<Course> studentsCourses = null;

            if (User.Identity.Name != null)
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    //Sql Query : 
                    //SELECT a.`UserID`, a.`CourseName`, a.`Subject`, a.`CourseDescription` , c.FirstName FROM `Course` a , `student` b , `user` c WHERE a.ID=b.CourseID And a.UserID=c.ID AND b.UserID = 6

                    studentsCourses = context.Courses.Include(x=>x.User).Where(x => x.Students.Any(y => y.CourseID == x.ID)).Where(x => x.Students.Any(y => y.UserID == int.Parse(User.Identity.Name))).ToList();
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


        /// <summary>
        /// Function to grab details of the signle course whose ID is provided by Student
        /// </summary>
        /// <param name="id"></param>
        /// <returns> details of a single course</returns>

        public Course GetCourseDetailsByID(string id) // Passing Course ID
        {
            ValidationException exception = new ValidationException();
            Course courseDetail = null;
            int parsedId;
            int userId = int.Parse(User.Identity.Name);

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            // check if id is non integer
            if (!int.TryParse(id, out parsedId))
            {
                exception.ValidationExceptions.Add(new Exception("Invalid ID , Go back to main Student Dashboard and select course again"));
            }
            else
            {            
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    // To confirm that student do no access any other students course 
                    // Select CourseID FROM student WHERE CourseID = -1 AND UserID = 6
                    Student confirmStudent = context.Students.Where(x => x.CourseID == parsedId && x.UserID == userId).SingleOrDefault();

                    if(confirmStudent!=null)
                    {
                        courseDetail = context.Courses.Where(x => x.ID == parsedId/* && x.UserID == userId*/).SingleOrDefault();
                    }
                    else
                    {
                        exception.ValidationExceptions.Add(new Exception("You are not registered in this course yet , Go back to main Student Dsahboard and select another course "));
                    }                 
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return courseDetail;
        }

        /// <summary>
        /// Function to get assignments for A course
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List of Assignments for a course </returns>
      /*  public List<Assignment> GetAssignmentsByCourseID(string id)
        {
            ValidationException exception = new ValidationException();
            List<Assignment> assignmentDetails = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("No Course ID Provided, Go back to main Student Dsahboard and select course again"));
            }
            else
            {
                if (int.TryParse(id, out parsedId))
                {
                    using (LearningManagementContext context = new LearningManagementContext())
                    {
                        assignmentDetails = context.Assignments.Where(x => x.CourseID == parsedId).ToList();
                    }
                }
                else
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to main Student Dsahboard and select course again"));
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return assignmentDetails;

        }*/


    }
}
