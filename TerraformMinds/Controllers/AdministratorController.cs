using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerraformMinds.Models;
using TerraformMinds.Models.Exceptions;

namespace TerraformMinds.Controllers
{
    /* DOC STRING 
     * @ Param =
     * 
     * @ summary = 
     * 
     * @ return = 
     */

    public class AdministratorController : Controller
    {
        ValidationException exception = new ValidationException();

        [Authorize(Roles = "Administrator")]
        public IActionResult AdministratorDashboard()
        {
            return View();
        }

        /********************************
         * ADMINISTRATOR COURSE COMMANDS
         ********************************/
        /// <summary>
        /// This CourseCreate is a GET function that will do two things:
        ///     1) Assign an instructor value based on the database
        ///     2) If an exception is encountered the values that were entered prior to submission will remain in the respect fields
        /// </summary>
        /// <param name="instructor"></param>
        /// <param name="courseName"></param>
        /// <param name="subject"></param>
        /// <param name="courseDescription"></param>
        /// <param name="gradeLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="currentCapacity"></param>
        /// <param name="maxCapacity"></param>
        /// <returns></returns>
        public IActionResult CourseCreate(string instructor, string courseName, string subject, string courseDescription, string gradeLevel, DateTime startDate, DateTime endDate, int currentCapacity, int maxCapacity)
        {
            using(LearningManagementContext context = new LearningManagementContext())
            {
                // Create a list based on first name and last name from the User table that has a role equal to 2 (which is "Instructor")
                var instructors = new SelectList(context.Users.Where(x => x.Role == 2)
                .OrderBy(y => y.LastName)
                .ToDictionary(us => us.ID, us => us.FirstName + " " + us.LastName), "Key", "Value", instructor);
                ViewBag.Instructors = instructors;
                ViewBag.CourseName = courseName;

                // Create list of Subjects
                var courseSubjects = new List<string>() { "English", "Math", "Science", "Social Studies" };
                // Populate dropdown list for subjects.
                var courseSubjectsList = new SelectList(courseSubjects.ToDictionary(s => s, s => s), "Key", "Value", courseSubjects);
                ViewBag.CourseSubjects = courseSubjectsList;

                ViewBag.Subject = subject;
                ViewBag.StartDate = startDate.ToLongDateString();
                ViewBag.EndDate = endDate.ToLongDateString();
                ViewBag.CourseDescription = courseDescription;

                // Create list of GradeLevels 
                var gradeLevels = new List<string>() { "Kindergarten", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12" };
                // Populate dropdown list for grade levels.
                var gradeLevelList = new SelectList(gradeLevels.ToDictionary(g => g, g => g), "Key", "Value", gradeLevel);
                ViewBag.GradeLevels = gradeLevelList;

                ViewBag.GradeLevel = gradeLevel;
                //ViewBag.CurrentCapacity = currentCapacity;
                ViewBag.MaxCapacity = maxCapacity; 
            }
            return View();
        }

        /// <summary>
        /// This CourseCreate is a POST function that will add the properties to the database
        /// 
        /// Validation occurs to check:
        /// 1) Current Capacity is LESS THAN Max Capacity
        /// 2) Start Date cannot be past the End Date
        /// 3) Cannot have a duplicate Course Name
        /// </summary>
        /// <param name="instructor"></param>
        /// <param name="courseName"></param>
        /// <param name="subject"></param>
        /// <param name="courseDescription"></param>
        /// <param name="gradeLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="currentCapacity"></param>
        /// <param name="maxCapacity"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        // POST: Course/CourseCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CourseCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseCreate(string instructor, string courseName, string subject, string courseDescription, string gradeLevel, DateTime startDate, DateTime endDate, int currentCapacity, int maxCapacity, [Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,MaxCapacity")] Course course)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {

                instructor = instructor != null ? instructor.Trim() : null;
                courseName = courseName != null ? courseName.Trim() : null;
                subject = subject != null ? subject.Trim() : null;
                courseDescription = courseDescription != null ? courseDescription.Trim() : null;

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (course.CurrentCapacity > course.MaxCapacity)
                        {
                            exception.ValidationExceptions.Add(new Exception("Invalid: Current Capacity cannot exceed Max Capacity."));
                        }
                        else
                        {
                            if (course.StartDate > course.EndDate)
                            {
                                exception.ValidationExceptions.Add(new Exception("Invalid: Course Start Date cannot be set past End Date."));
                            }

                            else
                            {
                                if (course.StartDate < DateTime.Today)
                                {
                                    exception.ValidationExceptions.Add(new Exception("Invalid: Course Start Date cannot be set in the past."));
                                }

                                else
                                {
                                    if (context.Courses.Any(x => x.CourseName == course.CourseName))
                                    {
                                        exception.ValidationExceptions.Add(new Exception("Invalid: That course already exists."));
                                    }
                                }
                            }
                        }

                        if (exception.ValidationExceptions.Count > 0)
                        {
                            throw exception;
                        }
                        context.Add(course);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(CourseList));
                    }
                    catch (ValidationException e)
                    {
                        ViewBag.Message = "There exist problem(s) with your submission, see below.";
                        ViewBag.Exception = e;
                        ViewBag.Error = true;
                    }

                    var instructors = new SelectList(context.Users.Where(x => x.Role == 2)
                    .OrderBy(y => y.LastName)
                    .ToDictionary(us => us.ID, us => us.FirstName + " " + us.LastName), "Key", "Value", instructor);
                    ViewBag.Instructors = instructors;
                    ViewBag.CourseName = courseName;

                    // Create list of Subjects
                    var courseSubjects = new List<string>() { "English", "Math", "Science", "Social Studies" };
                    // Populate dropdown list for subjects.
                    var courseSubjectsList = new SelectList(courseSubjects.ToDictionary(s => s, s => s), "Key", "Value", courseSubjects);
                    ViewBag.CourseSubjects = courseSubjectsList;

                    ViewBag.Subject = subject;
                    ViewBag.StartDate = startDate.ToLongDateString();
                    ViewBag.EndDate = endDate.ToLongDateString();
                    ViewBag.CourseDescription = courseDescription;

                    // Create list of GradeLevels 
                    var gradeLevels = new List<string>() { "Kindergarten", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12" };
                    // Populate dropdown list for grade levels.
                    var gradeLevelList = new SelectList(gradeLevels.ToDictionary(g => g, g => g), "Key", "Value", gradeLevel);
                    ViewBag.GradeLevels = gradeLevelList;

                    ViewBag.GradeLevel = gradeLevel;
                    //ViewBag.CurrentCapacity = currentCapacity;
                    ViewBag.MaxCapacity = maxCapacity;
                }
            }
            return View(course);
        }

        /// <summary>
        /// Populates a list of courses in View/CourseList 
        /// </summary>
        /// <returns>Showcases a list of courses in View.CourseList</returns>
        public IActionResult CourseList()
        {
            ViewBag.Courses = GetCourses();

            return View();
        }

        /// <summary>
        /// Creates a list of courses using LINQ queries to select information from the database.
        /// Used Include to join User table
        /// </summary>
        /// <returns> A list of Courses based on data from the database</returns>
        public List<Course> GetCourses()
        {
            List<Course> courseList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseList = context.Courses.Include(x => x.User).ToList();
            }
            return courseList;
        }

        /// <summary>
        /// Gets a Course based on its unique ID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns>Returns a course based on its unique ID</returns>
        public Course GetCourseByID(int courseID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (!context.Courses.Any(x => x.ID == courseID))
                {
                    exception.ValidationExceptions.Add(new Exception("That Course ID Does Not Exist"));
                    throw exception;
                }
                else
                {
                    Course course = context.Courses.Where(x => x.ID == courseID).Include(x => x.User).SingleOrDefault();

                    return course;
                }
            }
        }

        /// <summary>
        /// GET command that checks if the courseID is valid, if it is pre-populate input fields found in Views/CourseEdit
        /// based on database information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pre-populated fields in Views/CourseEdit with database information </returns>
        public async Task<IActionResult> CourseEdit(int? id)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (id == null)
                {
                return NotFound();
                }

                var course = await context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                // Create a list based on first name and last name from the User table that has a role equal to 2 (which is "Instructor")
                var instructors = new SelectList(context.Users.Where(x => x.Role == 2)
                    .OrderBy(y => y.LastName)
                    .ToDictionary(us => us.ID, us => us.FirstName + " " + us.LastName), "Key", "Value", course.UserID);
                ViewBag.Instructors = instructors;
                ViewBag.CourseName = course.CourseName;
                ViewBag.Subject = course.Subject;
                ViewBag.StartDate = course.StartDate;
                ViewBag.EndDate = course.EndDate;
                ViewBag.CourseDescription = course.CourseDescription;

                // Create list of GradeLevels 
                var gradeLevels = new List<string>() { "Kindergarten", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12" };
                // Populate dropdown list for grade levels.
                var gradeLevelList = new SelectList(gradeLevels.ToDictionary(g => g, g => g), "Key", "Value", course.GradeLevel);
                ViewBag.GradeLevels = gradeLevelList;

                ViewBag.GradeLevel = course.GradeLevel;
                ViewBag.CurrentCapacity = course.CurrentCapacity;
                ViewBag.MaxCapacity = course.MaxCapacity;

                return View();
            }
        }
        /// <summary>
        /// POST Command that will update the database with the changes to the course.
        /// Validation is include that checks:
        /// 1) Currenct Capacity cannot be greater than Max Capacity
        /// 2) Course Start Date cannot be ahead of Course End Date
        /// 3) Course Name is unique
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns>Updates the values for course in the database</returns>
        [HttpPost, ActionName("CourseEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseEdit(int id, [Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,CurrentCapacity,MaxCapacity")] Course course)
        {
            course.CourseName = course.CourseName != null ? course.CourseName.Trim() : null;
            course.Subject = course.Subject != null ? course.Subject.Trim() : null;
            course.CourseDescription = course.CourseDescription != null ? course.CourseDescription.Trim() : null;

            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (id != course.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (course.CurrentCapacity > course.MaxCapacity)
                        {
                            exception.ValidationExceptions.Add(new Exception("Error: Current Capacity cannot exceed Max Capacity."));
                        }
                        else
                        {
                            if (course.StartDate > course.EndDate)
                            {
                                exception.ValidationExceptions.Add(new Exception("Error: Course Start Date cannot be after End Date."));
                            }
                        }

                        if (exception.ValidationExceptions.Count > 0)
                        {
                            throw exception;
                        }

                        context.Update(course);
                        await context.SaveChangesAsync();
                        ViewBag.Message = $"Successfully updated course!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!(context.Courses.Any(x => x.ID == id)))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw exception;
                        }
                    }
                    catch (ValidationException e)
                    {
                        ViewBag.Message = "There exist problem(s) with your submission, see below.";
                        ViewBag.Exception = e;
                        ViewBag.Error = true;
                    }

                    // Create a list based on first name and last name from the User table that has a role equal to 2 (which is "Instructor")
                    var instructors = new SelectList(context.Users.Where(x => x.Role == 2)
                    .OrderBy(y => y.LastName)
                    .ToDictionary(us => us.ID, us => us.FirstName + " " + us.LastName), "Key", "Value", course.UserID);
                    ViewBag.Instructors = instructors;
                    ViewBag.CourseName = course.CourseName;
                    ViewBag.Subject = course.Subject;
                    ViewBag.StartDate = course.StartDate;
                    ViewBag.EndDate = course.EndDate;
                    ViewBag.CourseDescription = course.CourseDescription;

                    // Create list of GradeLevels 
                    var gradeLevels = new List<string>() { "Kindergarten", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12" };
                    // Populate dropdown list for grade levels.
                    var gradeLevelList = new SelectList(gradeLevels.ToDictionary(g => g, g => g), "Key", "Value", course.GradeLevel);
                    ViewBag.GradeLevels = gradeLevelList;

                    ViewBag.GradeLevel = course.GradeLevel;
                    ViewBag.CurrentCapacity = course.CurrentCapacity;
                    ViewBag.MaxCapacity = course.MaxCapacity;
                }   
            }
            return View(course);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CourseDelete(int? id)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var course = await context.Courses
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (course == null)
                {
                    return NotFound();
                }

                return View(course);
            }
        }

        [HttpPost, ActionName("CourseDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {                
                var course = await context.Courses.FindAsync(id);

                try
                {
                    if (course.StartDate < DateTime.Today)
                    {
                        exception.ValidationExceptions.Add(new Exception("Error: Cannot delete a course that is in progress."));
                    }

                    else if (course.EndDate > DateTime.Today && course.StartDate < DateTime.Today)
                    {
                        exception.ValidationExceptions.Add(new Exception("Error: Cannot delete a course that is in progress."));
                    }

                    if (exception.ValidationExceptions.Count > 0)
                    {
                        throw exception;
                    }
                    context.Courses.Remove(course);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(CourseList));
                }

                catch (ValidationException e)
                {
                    ViewBag.Message = "There exist problem(s) with your submission, see below.";
                    ViewBag.Exception = e;
                    ViewBag.Error = true;
                }
                return View(course);
            }
        }

        /************************************
         * ADMINISTRATOR INSTRUCTOR COMMANDS
         ************************************/

        public IActionResult InstructorDetail(int instructorID)
        {
            try
            {
                User instructor = GetInstructorByID(instructorID);
                ViewBag.Instructor = instructor;
                ViewBag.InstructorCourses = GetCoursesByInstructorID(instructor.ID);
            }
            catch(ValidationException e)
            {
                ViewBag.Message = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }
            return View();
        }

        public IActionResult InstructorList()
        {
            ViewBag.Instructors = GetInstructors();

            return View();
        }

        public List<User> GetInstructors()
        {
            List<User> instructorList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                instructorList = context.Users.Where(x => x.Role == 2).ToList();
            }
            return instructorList;
        }

        public User GetInstructorByID(int instructorID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if(!context.Users.Any(x => x.ID == instructorID))
                {
                    exception.ValidationExceptions.Add(new Exception("Error: Cannot find Instructor"));
                    throw exception;
                }

                User instructor = context.Users.Where(x => x.ID == instructorID).Include(x => x.Courses).SingleOrDefault();
                return instructor;
            }
        }

        public List<Course> GetCoursesByInstructorID(int userID)
        {
            List<Course> teachingCourses;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                teachingCourses = context.Courses.Where(x => x.UserID == userID).Include(x => x.User).ToList();

                return teachingCourses;
            }
        }

        /************************************
        * ADMINISTRATOR STUDENT COMMANDS
        ************************************/
        public IActionResult StudentDetail(int studentID)
        {
            try
            {
                Student student = GetStudentByID(studentID);
                ViewBag.Student = student;
                ViewBag.StudentCourses = GetEnrolledCoursesByStudentID(student.UserID);

            }
            catch (ValidationException e)
            {
                ViewBag.Message = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }
            return View();
        }

        public IActionResult StudentList()
        {
            ViewBag.Students = GetStudents();

            return View();
        }

        public Student GetStudentByID(int studentID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (!context.Students.Any(x => x.ID == studentID))
                {
                    exception.ValidationExceptions.Add(new Exception("Error: Cannot find Student"));
                    throw exception;
                }

                Student student = context.Students.Where(x => x.ID == studentID).Include(x => x.User).SingleOrDefault();
                return student;
            }
        }

        public List<Course> GetEnrolledCoursesByStudentID(int userID)
        {
            List<Course> enrolledCourses;
            List<Student> studentCourses;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                studentCourses = context.Students.Where(x => x.UserID == userID).ToList();

                // Find unique list of course IDs that the student is enrolled in.
                List<int> courseIDs = new List<int>();
                foreach (Student student in studentCourses)
                {
                    if (!courseIDs.Contains(student.CourseID))
                    {
                        courseIDs.Add(student.CourseID);
                    }
                }

                // Get all additional course info that the student is enrolled in.
                enrolledCourses = context.Courses.Where(x => courseIDs.Contains(x.ID)).Include(x => x.User).ToList();
                return enrolledCourses;
            }
        }

        public List<Student> GetStudents()
        {
            List<Student> studentList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                studentList = context.Students.Where(x => x.User.Role == 3).Include(x => x.User).ToList();
            }
            return studentList;
        }
    }
}
