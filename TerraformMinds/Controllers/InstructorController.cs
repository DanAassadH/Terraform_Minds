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
        /// <summary>
        /// Action to display signed in user's dashboard
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult InstructorDashboard()
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
        /// Action for course List page for individual instructor , it shows that how many courses that instructor has
        /// </summary>
        /// <param name="id"></param>
        /// <returns> View() </returns>

        [Authorize(Roles = "Instructor")]
        public IActionResult CourseList()
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                ViewBag.InstructorsCourses = GetCourseByInstructorID(User.Identity.Name);
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
        /// Action for Course details page this page gives detail about student and assignments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult CourseDetail(string id) // passing CourseID
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                ViewBag.SingleCourseDetail = GetCourseDetailsByID(id);
                if (ViewBag.SingleCourseDetail != null)
                {
                    // Get students enrolled in this course
                    ViewBag.StudentsForCourse = GetStudentsByCourseID(id);
                    ViewBag.AssignmentsForCourse = GetAssignmentsByCourseID(id);
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
        /// Action to create a new assignment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentCreate(string Question, string DueDate, string TotalScore, string id) // passing CourseID
        {

            ViewBag.PassingCourseID = id;

            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                if (Request.Method == "POST")
                {
                    CreateNewAssignment(Question, DueDate, TotalScore, id);
                    ViewBag.Message = $"Successfully Created Assignment!";
                    ViewBag.AssignmentCreated = true;
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
        /// Action to Display all the Assignments of a student for a course in teacher dashboard
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <returns>Assignments List Page</returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentList(string cid, string uid)
        {

            ViewBag.CourseId = cid;

            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                int studentId = StudentController.GetStudentId(cid, uid);
                /* Call function to get student name ~ */
                ViewBag.SubmittedAssignments = StudentController.GetSubmittedAssignments(studentId.ToString());

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
        /// Action to display the submitted assignment to instructor , and let instructor add score and remarkds on that answer
        /// </summary>
        /// <param name="submitId"></param>
        /// <param name="Remarks"></param>
        /// <param name="ScoreObtained"></param>
        /// <returns>View of the assignment answer </returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentMark(string submitId, string Remarks, string ScoreObtained, string TotalScore)
        {

            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                ViewBag.SubmittedAssignmentAnswer = GetSubmittedAssignmentBySubmitID(submitId);

                if (Request.Method == "POST")
                {
                    SubmitAssignmentScoreAndRemarks(submitId, Remarks, ScoreObtained, TotalScore);
                    ViewBag.Message = $"Successfully Submitted Assignment!";
                    ViewBag.SubmitYes = true;
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
        /// This function grabs course List for individual instructor
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of courses</returns>
        public List<Course> GetCourseByInstructorID(string UserId)
        {
            ValidationException exception = new ValidationException();
            List<Course> instructorsCourses = null;

            if (UserId != null)
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    instructorsCourses = context.Courses.Where(x => x.UserID == int.Parse(UserId)).ToList();
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

        public Course GetCourseDetailsByID(string id) // Passing Course ID
        {
            ValidationException exception = new ValidationException();
            Course courseDetail = null;
            int parsedId;
            int userId = int.Parse(User.Identity.Name);

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            // check if id is non integer

            if (int.TryParse(id, out parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {

                    courseDetail = context.Courses.Where(x => x.ID == parsedId && x.UserID == userId).SingleOrDefault();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid ID : Go back to main Instructor Dsahboard and select course again"));
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


            if (int.TryParse(id, out parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {

                    // sql query : get all the students enrolled in this course
                    //SELECT a.FirstName, a.LastName FROM `user` a , student b WHERE a.ID=b.UserID AND CourseID = -2(id)

                    studentNames = context.Users.Where(x => x.Students.Any(y => y.UserID == x.ID)).Where(x => x.Students.Any(y => y.CourseID == parsedId)).ToList();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Course ID : Go back to main Instructor Dsahboard and select course again"));
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
        public static List<Assignment> GetAssignmentsByCourseID(string id) //Passing Course ID
        {
            ValidationException exception = new ValidationException();
            List<Assignment> assignmentDetails = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;


            if (int.TryParse(id, out parsedId))
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    assignmentDetails = context.Assignments.Where(x => x.CourseID == parsedId).OrderBy(x => x.DueDate).ToList();
                }
            }
            else
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Course ID : Go back to main Dsahboard and select course again"));
            }


            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return assignmentDetails;

        }

        /// <summary>
        /// Function to insert assignment values by instructor into assignment table 
        /// Validation # 1 : Due Date for assignment Cannot be before Course start date
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
            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid ID : Go back to details page and try again"));
                flag = true;
            }
            else
            {
                if (!int.TryParse(id, out parsedId))
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID : Go back to main Instructor Dsahboard and select course again"));
                    flag = true;
                }
            }
            
            // Validation for question
            if (question == null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Value : Question Required"));
                flag = true;
            }
            else
            {
                if (question.Length > 500)
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid character count : cannot exceed 500 characters, please rephrase"));
                    flag = true;
                }
            }

            // Validation for dueDate
            if (dueDate == null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Date : Due Date Required"));
                flag = true;
            }
            else
            {
                if (DateTime.Parse(dueDate) < DateTime.Now)
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Date : Due Date Can not be set prior to today"));
                    flag = true;
                }
            }

            // Validation for TotalScore
            if (totalScore == null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Score : total score for assignment required"));
                flag = true;
            }
            else
            {
                if (!int.TryParse(totalScore, out parsedTotalScore))
                {
                    exception.ValidationExceptions.Add(new Exception("Invalid Value : Numeric Value required for total score"));
                    flag = true;
                }
                else
                {
                    if (!((parsedTotalScore > -1) && (parsedTotalScore < 101)))
                    {
                        exception.ValidationExceptions.Add(new Exception("Invalid value : enter total score between 0 and 100"));
                        flag = true;
                    }
                }

            }


            if (flag == false)
            {

                using (LearningManagementContext context = new LearningManagementContext())
                {
                    Course course = GetCourseDetailsByID(id);

                    if(DateTime.Parse(dueDate) < course.StartDate)
                    {
                        exception.ValidationExceptions.Add(new Exception("Invalid Due date : due date for an assignment cannot be before course start date"));
                        flag = true;
                    }

                    if(flag == false)
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

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

        }

        /// <summary>
        /// Function to get the answer of an assignment submitted by student
        /// </summary>
        /// <param name="submitID"></param>
        /// <returns>Students Submitted answer to assignment Question</returns>
        public Submit GetSubmittedAssignmentBySubmitID(string submitID)
        {
            ValidationException exception = new ValidationException();
            Submit studentsAssignment = null;

            submitID = submitID?.Trim();

            int parsedId;

            // Validation for submitID
            if (!int.TryParse(submitID, out parsedId))
            {
                exception.ValidationExceptions.Add(new Exception("Invalid Course ID : Go back to course page and try again"));

            }
            else
            {
                using (LearningManagementContext context = new LearningManagementContext())
                {
                    //Sql Query : 
                    // SELECT a.* , b.* FROM `submitted` a , `assignment` b WHERE a.AssignmentID = b.ID And a.ID = 5

                    studentsAssignment = context.Submissions.Include(x => x.Assignment).Where(x => x.Assignment.ID == x.AssignmentID).Where(x => x.ID == int.Parse(submitID)).SingleOrDefault();
                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

            return studentsAssignment;
        }

        /// <summary>
        /// Function to update the Score and remarks in submit, table these fields are added by instructor
        /// </summary>
        /// <param name="submitId"></param>
        /// <param name="remarks"></param>
        /// <param name="scoreObtained"></param>
        /// <param name="totalScore"></param>
        public void SubmitAssignmentScoreAndRemarks(string submitId, string remarks, string scoreObtained, string totalScore)
        {
            ValidationException exception = new ValidationException();
            Submit updateSubmission = null;
            submitId = submitId?.Trim();
            remarks = remarks?.Trim();
            scoreObtained = scoreObtained?.Trim();
            totalScore = totalScore?.Trim();

            bool flag = false;

            int parsedId;
            int parsedScoreObtained;
            int parsedTotalScore;

            // Validation for submitID
            if (!int.TryParse(submitId, out parsedId))
            {
                exception.ValidationExceptions.Add(new Exception("Invalid submission ID : go back to course list and try again"));
                flag = true;
            }

            // Validation for Total Score
            if (!int.TryParse(totalScore, out parsedTotalScore))
            {
                exception.ValidationExceptions.Add(new Exception("Invalid value: total score required"));
                flag = true;
            }


            // Validation for remarks
            if (remarks.Length > 500 && remarks != null)
            {
                exception.ValidationExceptions.Add(new Exception("Invalid value : cannot exceed character count of 500, please rephrase"));
                flag = true;
            }

            // Validation for Score obtained
            if (!int.TryParse(scoreObtained, out parsedScoreObtained))
            {
                exception.ValidationExceptions.Add(new Exception("Invalud value : numeric Value required for score obtained"));
                flag = true;
            }
            else
            {
                if (!((parsedScoreObtained > -1) && (parsedScoreObtained < parsedTotalScore + 1)))
                {
                    exception.ValidationExceptions.Add(new Exception($"Invalid value : enter total score between 0 and {parsedTotalScore}"));
                    flag = true;
                }
            }


            if (flag == false)
            {

                using (LearningManagementContext context = new LearningManagementContext())
                {

                    updateSubmission = context.Submissions.Where(x => x.ID == int.Parse(submitId)).SingleOrDefault();
                    updateSubmission.ScoreObtained = int.Parse(scoreObtained);
                    updateSubmission.Remarks = remarks;
                    context.SaveChanges();

                }
            }

            if (exception.ValidationExceptions.Count > 0)
            {
                throw exception;
            }

        }
    }
}
