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
        /// <summary>
        /// Action to display Signed In User dashboard
        /// </summary>
        /// <returns>main dashboard view </returns>
        [Authorize(Roles = "Student")]
        public IActionResult StudentDashboard()
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }

            return View();

        }

        /// <summary>
        /// This is the Action for course List page for individual student , it shows that how many courses that student is enrolled in
        /// </summary>
        /// <param name="id"></param>
        /// <returns> View() </returns>

        [Authorize(Roles = "Student")]
        public IActionResult CourseList()
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                ViewBag.StudentsCourses = GetCourseByStudentID();
            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }

            return View();
        }

        /// <summary>
        /// Action for Course details page this page gives detail about a Course and its assignments 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student")]
        public IActionResult CourseDetail(string id) // passing CourseID
        {
            List<Submit> submittedAssignments;
        
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                ViewBag.SingleCourseDetail = GetCourseDetailsByID(id);
                if (ViewBag.SingleCourseDetail != null)
                {
                    int studentId = GetStudentId(id,User.Identity.Name);
                    submittedAssignments = GetSubmittedAssignments(studentId.ToString());
                   
                    List<int> submittedIds = new List<int>();

                    int i = 0;
                    foreach (Submit submit in submittedAssignments)
                    {
                        submittedIds.Add(submit.AssignmentID) ;
                        i++;
                    }

                      // Get Assignments for this course Calling this function from instructor controller
                    ViewBag.AssignmentsForCourse = InstructorController.GetAssignmentsByCourseID(id);
                    ViewBag.SubmittedAssignments = submittedAssignments;
                    ViewBag.SubmittedIds = submittedIds;

                }
            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }

            return View();
        }

        /// <summary>
        /// Action to Display Individual Assignment Page to Student , student can submit the answer of a question 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Answer"></param>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student")]
        public IActionResult AssignmentAttempt(string id, string Answer, string CourseID) // id is Assignment id
        {

            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);

                if (Request.Method == "POST")
                {
                    SubmitAssignment(id, Answer, CourseID);
                    ViewBag.SuccessMessage = $"Successfully Submitted Assignment!";
                    ViewBag.SubmitYes = true;
                    ViewBag.BackCourseID = CourseID;
                }
                else
                {
                    ViewBag.AssignmentDetails = GetAssignmentByID(id);
                }

            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }

            return View();
        }

        /* ------------------------------------------Data -----------------------------------------------------*/
        /// <summary>
        /// This function grabs course List for individual Student
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
                    //SELECT a.`UserID`, a.`CourseName`, a.`Subject`, a.`CourseDescription` , c.FirstName FROM `Course` a , `student` b , `user` c WHERE a.ID=b.CourseID And a.UserID=c.ID AND b.UserID = 6(id)

                    studentsCourses = context.Courses.Include(x => x.User).Where(x => x.Students.Any(y => y.CourseID == x.ID)).Where(x => x.Students.Any(y => y.UserID == int.Parse(User.Identity.Name))).OrderBy(x=>x.Subject).ToList();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid operation : please logout and try again"));
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
                exception.ValidationExceptions.Add(new Exception("Invalid ID : course details not available, please go back to Main dashboard and try again"));
            }
            else
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    // To confirm that student do no access any other students course 
                    // Select CourseID FROM student WHERE CourseID = -1 AND UserID = 6
                    Student confirmStudent = context.Students.Where(x => x.CourseID == parsedId && x.UserID == userId).SingleOrDefault();

                    if (confirmStudent != null)
                    {
                        courseDetail = context.Courses.Where(x => x.ID == parsedId).SingleOrDefault();
                    }
                    else
                    {
                        exception.ValidationExceptions.Add(new Exception("Invalid operation : you are not enrolled in this course"));
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
        /// Function to get Details of an assignment
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Assignment details</returns>
        public Assignment GetAssignmentByID(string id) // Passing Assignment ID
        {
            ValidationException exception = new ValidationException();
            Assignment assignmentDetails = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;


            if (int.TryParse(id, out parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    assignmentDetails = context.Assignments.Where(x => x.ID == parsedId).SingleOrDefault();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Assignemnt ID : go back to main Student Dashboard and select again"));
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return assignmentDetails;

        }

        /// <summary>
        /// Function to enter the Answer of an assignment in the database by Student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="answer"></param>
        /// <param name="courseID"></param>
        public void SubmitAssignment(string id, string answer, string courseID)
        {
            ValidationException exception = new ValidationException();
            bool flag = false;
            int parsedId;

            // Trim the values 
            id = id?.Trim();
            answer = answer?.Trim();
            courseID = courseID?.Trim();

            // Validation for  Assignment ID
            if (id == null || courseID == null)
            {
                exception.ValidationExceptions.Add(new Exception("ID not found, Go back to details page and try again"));
                flag = true;
            }
            else
            {
                if (!int.TryParse(id, out parsedId))
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID : Go back to main instructor dsahboard and select course again"));
                    flag = true;
                }
            }

            // Validation for answer
            if (answer == null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Value : Answer Required"));
                flag = true;
            }
            else
            {
                if (answer.Length > 2000)
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid value : cannot exceed character count of 2000, please rephrase"));
                    flag = true;
                }
            }


            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }


            if (flag == false)
            {

                using (LearningManagementContext context = new LearningManagementContext())
                {

                    // Add Values in assignment Table if all validations are passed
                    context.Submissions.Add(new Submit()
                    {
                        AssignmentID = int.Parse(id),
                        StudentID = GetStudentId(courseID,User.Identity.Name),
                        DateSubmitted = DateTime.Now,
                        Answer = answer

                    });
                    context.SaveChanges();

                }

            }
        }

        /// <summary>
        /// function to get student id of a student which is a unique course - user(with role 3) key pair
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns>student Id</returns>
        public static int GetStudentId(string courseID , string userID)
        {
            ValidationException exception = new ValidationException();
            int id = -100 ; // setting a ID value that dont exist in database


            using (LearningManagementContext context = new LearningManagementContext())
            {
               Student studentID = context.Students.Where(x => x.CourseID == int.Parse(courseID) && x.UserID==int.Parse(userID)).SingleOrDefault();

                if(studentID == null)
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid operation : Unable to access this record"));
                }
                else
                {
                    id = studentID.ID;
                }              
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return id;
        }


        /// <summary>
        /// Function to get the list of submitted assignments for a course for student
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns>List of submitted assignments of a course</returns>

        public static List<Submit> GetSubmittedAssignments(string studentID)
        {
            ValidationException exception = new ValidationException();
            List<Submit> studentsAssignment = null;
            int parsedId;

            if (int.TryParse(studentID, out parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    //Sql Query : 
                    //SELECT a.* , b.* FROM `submitted` a , `assignment` b WHERE a.AssignmentID = b.ID AND a.StudentID = 5 (id)

                    studentsAssignment = context.Submissions.Include(x => x.Assignment).Where(x => x.Assignment.ID == x.AssignmentID).Where(x => x.StudentID == parsedId).OrderBy(x=>x.DateSubmitted).ToList();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid operation : please go to main dashboard and try again"));
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return studentsAssignment;
        }



    }
}
