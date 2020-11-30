using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                ViewBag.InstructorsCourses = GetCourseByUserID(id);
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
        public IActionResult CourseDetail(string id) // passing CourseID
        {
            try
            {
                ViewBag.SingleCourseDetail = GetCourseDetailsByID(id);
                if(ViewBag.SingleCourseDetail != null)
                {
                    // Get students enrolled in this course
                    ViewBag.StudentsForCourse = GetStudentsByCourseID(id);
                    ViewBag.AssignmentsForCourse = GetAssignmentsByCourseID(id);
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


        /// <summary>
        /// Action to create a new assignment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentCreate(string Question, string DueDate, string TotalScore, string id) // passing CourseID
        {
  
            ViewBag.PassingCourseID = id;
            if (Request.Method == "POST")
            {
                try
                {
                    CreateNewAssignment(Question, DueDate, TotalScore, id);
                    ViewBag.Message = $"Successfully Created Assignment!";
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


        /* ------------------------------------------Data -----------------------------------------------------*/
        /// <summary>
        /// This function grabs course List for individual instructor
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of courses</returns>
        public List<Course> GetCourseByUserID(string id)
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
        /// Function to grab details of the signle course whose ID is provided by instructor
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


        /// <summary>
        /// Gets List of students in a course for the instructor
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of Students</returns>
        public List<User> GetStudentsByCourseID(string id)
        {
            ValidationException exception = new ValidationException();
            List<User> studentNames = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("No Course ID Provided, Go back to main Instructor Dsahboard and select course again"));
            }
            else
            {
                if (int.TryParse(id, out parsedId))
                {
                    using (LearningManagementContext context = new LearningManagementContext())
                    {


                        //  For reference for now :  allBooks = context.Books.Include(x => x.Author).Include(x => x.Borrows).Where(x => x.Borrows.Any(y => y.DueDate < DateTime.Today && y.ReturnedDate == null)).ToList();

                        // sql query : get all the students enrolled in this course
                        //SELECT a.FirstName, a.LastName FROM `user` a , student b WHERE a.ID=b.UserID AND CourseID = -2(id)

                          studentNames = context.Users.Where(x => x.Students.Any(y => y.UserID == x.ID)).Where(x => x.Students.Any(y => y.CourseID == parsedId)).ToList();
                    }
                }
                else
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to main Instructor Dsahboard and select course again"));
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return studentNames;

        }

        /// <summary>
        /// Function to get assignments for A course
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List of Assignments for a course </returns>
        public List<Assignment> GetAssignmentsByCourseID(string id)
        {
            ValidationException exception = new ValidationException();
            List<Assignment> assignmentDetails = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("No Course ID Provided, Go back to main Instructor Dsahboard and select course again"));
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
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to main Instructor Dsahboard and select course again"));
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return assignmentDetails;

        }

        /// <summary>
        /// Function to insert assignment values by instructor into assignment table
        /// </summary>
        /// <param name="question"></param>
        /// <param name="dueDate"></param>
        /// <param name="totalScore"></param>
        /// <param name="id"></param>
        public void CreateNewAssignment(string question, string dueDate, string totalScore, string id)
        {
            ValidationException exception = new ValidationException();

            // Trim the values 
            question = question?.Trim();
            dueDate = dueDate?.Trim();
            totalScore = totalScore?.Trim();
            id = id?.Trim();

            bool flag = false;

            int parsedId;
            int parsedTotalScore;

            // Validation for courseID
            if(id==null)
            {
                exception.ValidationExceptions.Add(new Exception("ID not found, Go back to details page and try again"));
                flag = true;
            }
            else
            {
                if (!int.TryParse(id, out parsedId))
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to main Instructor Dsahboard and select course again"));
                    flag = true;
                }
            }

            // Validation for question
            if (question == null)
            {
                exception.ValidationExceptions.Add(new Exception("Question Required"));
                flag = true;
            }
            else
            {
                if(question.Length>500)
                {
                    exception.ValidationExceptions.Add(new Exception("exceed character count of 500, please rephrase"));
                    flag = true;
                }
            }

            // Validation for dueDate
            if (dueDate == null)
            {
                exception.ValidationExceptions.Add(new Exception("Due Date Required"));
                flag = true;
            }
            else
            {
                if(DateTime.Parse(dueDate)<DateTime.Now)
                {
                    exception.ValidationExceptions.Add(new Exception("Due Date Can not be before today"));
                    flag = true;
                }
            }

            // Validation for TotalScore
            if (totalScore == null)
            {
                exception.ValidationExceptions.Add(new Exception("Total Score for Assignment required"));
                flag = true;
            }
            else
            {
                if (!int.TryParse(totalScore, out parsedTotalScore))
                {
                    exception.ValidationExceptions.Add(new Exception("Numeric Value required for Total Score"));
                    flag = true;
                }
                else
                {
                    if (!((parsedTotalScore > -1) && (parsedTotalScore < 101)))
                    {
                        exception.ValidationExceptions.Add(new Exception("Enter Total Score between 0 and 100"));
                        flag = true;
                    }
                }

            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

             
            if (flag==false)
            {
        
                using (LearningManagementContext context = new LearningManagementContext())
                {
                 
                    // Add Values in assignment Table if all validations are passed
                    context.Assignments.Add(new Assignment()
                    {
                        CourseID = int.Parse(id),
                        Question = question,
                        DueDate = DateTime.Parse(dueDate),
                        TotalScore = int.Parse(totalScore)
                       
                    });
                    context.SaveChanges();

                }

            }

        }
    }
}
