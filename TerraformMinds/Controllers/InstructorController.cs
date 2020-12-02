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
        /// Action for course List page for individual instructor , it shows that how many courses that instructor has
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

        /// <summary>
        /// Action to Display all the Assignments of a student for a course in teacher dashboard
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <returns>Assignments List Page</returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentList(string cid, string uid ) 
        {

            try
            {
                int studentId = StudentController.GetStudentId(cid, uid);
                ViewBag.SubmittedAssignments = StudentController.GetSubmittedAssignments(studentId);

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
        /// Action to display the submitted assignment to instructor , and let instructor add score and remarkds on that answer
        /// </summary>
        /// <param name="submitId"></param>
        /// <param name="Remarks"></param>
        /// <param name="ScoreObtained"></param>
        /// <returns>View of the assignment answer </returns>
        [Authorize(Roles = "Instructor")]
        public IActionResult AssignmentMark(string submitId , string Remarks ,string ScoreObtained, string TotalScore)
        {

            try
            {
                ViewBag.SubmittedAssignmentAnswer = GetSubmittedAssignmentBySubmitID(submitId);

                if (Request.Method=="POST")
                {
                    SubmitAssignmentScoreAndRemarks(submitId, Remarks, ScoreObtained,TotalScore);
                    ViewBag.Message = $"Successfully Submitted Assignment!";
                    ViewBag.SubmitYes = true;
                }
/*                else
                {
                    ViewBag.SubmittedAssignmentAnswer = GetSubmittedAssignmentBySubmitID(submitId);
                }*/

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
        public static List<Assignment> GetAssignmentsByCourseID(string id)
        {
            ValidationException exception = new ValidationException();
            List<Assignment> assignmentDetails = null;

            int parsedId;

            id = !string.IsNullOrWhiteSpace(id) ? id.Trim() : null;

            if (id == null)
            {
                exception.ValidationExceptions.Add(new Exception("No Course ID Provided, Go back to main Dsahboard and select course again"));
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
                    exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to main Dsahboard and select course again"));
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
               exception.ValidationExceptions.Add(new Exception("Invalid Course ID , Go back to course page and try again"));
 
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
        public void SubmitAssignmentScoreAndRemarks(string submitId,string  remarks,string scoreObtained, string totalScore)
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
                    exception.ValidationExceptions.Add(new Exception("Invalid submission ID , Go back to Course list and try again"));
                    flag = true;
                }

            // Validation for submitID
            if (!int.TryParse(totalScore, out parsedTotalScore))
            {
                exception.ValidationExceptions.Add(new Exception("TotalScore not provided , Go back to Course list and try again"));
                flag = true;
            }


            // Validation for remarks
            if (remarks.Length > 500 && remarks!=null)
                {
                    exception.ValidationExceptions.Add(new Exception("Exceed character count of 500, please rephrase"));
                    flag = true;
                }

            // Validation for Score obtained
                if (!int.TryParse(scoreObtained, out parsedScoreObtained))
                {
                    exception.ValidationExceptions.Add(new Exception("Numeric Value required for Score Obtained"));
                    flag = true;
                }
                else
                {
                    if (!((parsedScoreObtained > -1) && (parsedScoreObtained < parsedTotalScore+1)))
                    {
                        exception.ValidationExceptions.Add(new Exception($"Enter Total Score between 0 and {parsedTotalScore}"));
                        flag = true;
                    }
                }


            if (flag == false)
            {

                using (LearningManagementContext context = new LearningManagementContext())
                {

                    updateSubmission = context.Submissions.Where(x => x.ID == int.Parse(submitId)).SingleOrDefault();
/*
                    context.Update(course);
                    await context.SaveChangesAsync();
                    ViewBag.Message = $"Successfully updated course!";*/

                  //  updateSubmission = await context.Submissions.FindAsync(int.Parse(submitId));

                    updateSubmission.ScoreObtained = int.Parse(scoreObtained);
                    updateSubmission.Remarks = remarks;
                   // context.Update(updateSubmission);
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
