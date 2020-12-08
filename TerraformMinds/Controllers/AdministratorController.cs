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
    public class AdministratorController : Controller
    {
        ValidationException exception = new ValidationException();

        /* ------------------------------------------Administrator Main Actions -----------------------------------------------------*/

        /// <summary>
        /// Administrator Dashboard View
        /// </summary>
        /// <returns>Goes to Administrator Dashboard View</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult AdministratorDashboard()
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            return View();
        }

        /********************************
         * ADMINISTRATOR COURSE ACTIONS
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
        /// <returns>Populates input fields, if properties were found</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult CourseCreate(string instructor, string courseName, string subject, string courseDescription, string gradeLevel, DateTime? startDate, DateTime? endDate, int currentCapacity, int maxCapacity)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            using (LearningManagementContext context = new LearningManagementContext())
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
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
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
        /// 1) Current Capacity is LESS THAN Maximum Capacity
        /// 2) Start Date cannot be past the End Date
        /// 3) End Date cannot be past todays date
        /// 4) Start Date cannot be prior to todays date
        /// 5) Cannot have a duplicate Course Name
        /// 6) Maximum capacity cannot be <= 0
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
        /// <returns>Updates database if successful; populates input fields if errors founds</returns>
        // POST: Course/CourseCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CourseCreate")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CourseCreate(string instructor, string courseName, string subject, string courseDescription, string gradeLevel, DateTime? startDate, DateTime? endDate, int currentCapacity, int maxCapacity, [Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,MaxCapacity")] Course course)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
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
                        if(course.MaxCapacity <= 0)
                        {
                            exception.ValidationExceptions.Add(new Exception("Invalid Maximum Capacity: Course must have a minimum of 1 student"));
                        }
                        else
                        {
                            if (course.CurrentCapacity > course.MaxCapacity)
                            {
                                exception.ValidationExceptions.Add(new Exception("Invalid Current Capacity: Current capacity cannot exceed maximum capacity"));
                            }
                            else
                            {
                                if (course.StartDate > course.EndDate)
                                {
                                    exception.ValidationExceptions.Add(new Exception("Invalid Start Date: Course start date cannot be set past the end date"));
                                }

                                else
                                {
                                    if (course.StartDate < DateTime.Today)
                                    {
                                        exception.ValidationExceptions.Add(new Exception("Invalid Start Date: Course start date cannot be set prior to todays date"));
                                    }

                                    else
                                    {
                                        if(course.EndDate < DateTime.Today)
                                        {
                                            exception.ValidationExceptions.Add(new Exception("Invalid End Date: Course end date cannot be set prior to todays date"));
                                        }

                                        else
                                        {
                                            if (context.Courses.Any(x => x.CourseName == course.CourseName))
                                            {
                                                exception.ValidationExceptions.Add(new Exception("Invalid Course Name: That course already exists"));
                                            }
                                        }
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
                        ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
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
                    ViewBag.StartDate = startDate;
                    ViewBag.EndDate = endDate;
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
        /// Populates a list of courses in CourseList View
        /// Course List view can change based on filters selected.
        /// Course List can be filtered by Grade Level, Subject or both
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses in CourseList View</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult CourseList(string subjectFilter, string gradeFilter)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            var gradeLevels = new List<string>() { "Kindergarten", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12" };
            var gradeLevelList = new SelectList(gradeLevels.ToDictionary(g => g, g => g), "Key", "Value", gradeLevels);
            ViewBag.GradeLevels = gradeLevelList;

            var courseSubjects = new List<string>() { "English", "Math", "Science", "Social Studies" };
            var courseSubjectsList = new SelectList(courseSubjects.ToDictionary(s => s, s => s), "Key", "Value", courseSubjects);
            ViewBag.CourseSubjects = courseSubjectsList;

            if (gradeLevels.Contains(gradeFilter) && !courseSubjects.Contains(subjectFilter))
            {
                ViewBag.Courses = GetCoursesByGrade(gradeFilter);
                ViewBag.Filter = true;
            }

            else if (courseSubjects.Contains(subjectFilter) && !gradeLevels.Contains(gradeFilter))
            {
                ViewBag.Courses = GetCoursesBySubject(subjectFilter);
                ViewBag.Filter = true;
            }

            else if (courseSubjects.Contains(subjectFilter) && gradeLevels.Contains(gradeFilter))
            {
                ViewBag.Courses = GetCoursesByGradeAndSubject(subjectFilter, gradeFilter);
                ViewBag.Filter = true;
            }

            else
            {
                ViewBag.Courses = GetCoursesAll();
                ViewBag.Filter = false;
            }
            return View("CourseList");
        }

        /// <summary>
        /// GET command that checks if the courseID is valid, if it is valid pre-populate input fields found in Views/CourseEdit
        /// based on database information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pre-populated fields in Views/CourseEdit with database information </returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CourseEdit(int? id)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var course = await context.Courses.FindAsync(id);
                if (course == null)
                {
                    //return NotFound();
                    ViewBag.CourseExists = false;
                    return View(course);
                }

                ViewBag.CourseExists = true;

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CourseEdit(int id, [Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,CurrentCapacity,MaxCapacity")] Course course)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
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
                        if (course.MaxCapacity <= 0)
                        {
                            exception.ValidationExceptions.Add(new Exception("Invalid Maximum Capacity: Course must have a minimum of 1 student."));
                        }
                        else
                        {
                            if (course.CurrentCapacity > course.MaxCapacity)
                            {
                                exception.ValidationExceptions.Add(new Exception("Invalid Current Capacity: Current capacity cannot exceed maximum capacity."));
                            }
                            else
                            {
                                if (course.StartDate > course.EndDate)
                                {
                                    exception.ValidationExceptions.Add(new Exception("Invalid Start Date: Course start date cannot be after end date."));
                                }
                            }
                        }

                        if (exception.ValidationExceptions.Count > 0)
                        {
                            throw exception;
                        }

                        ViewBag.CourseExists = true;

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
                        ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
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
            return View();
        }

        /// <summary>
        /// GET command that checks if the course id is valid, if it is valid pre-populate input fields found in Views/CourseEdit
        /// based on database information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Populated data based on course id</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CourseDelete(int? id)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
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
                    //return NotFound();
                    ViewBag.CourseExists = false;
                    return View();
                }

                ViewBag.CourseExists = true;
                return View(course);
            }
        }
        /// <summary>
        /// POST: Deletes the course record. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deletes the course record</returns>
        [HttpPost, ActionName("CourseDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
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

        /* ------------------------------------------Administrator Main Data -----------------------------------------------------*/

        /// <summary>
        /// Creates a list of courses using LINQ queries to select course information from the database.
        /// Used Include to join User table
        /// </summary>
        /// <returns> A list of Courses based on data from the database</returns>
        public List<Course> GetCoursesAll()
        {
            List<Course> courseList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseList = context.Courses.Include(x => x.User).OrderBy(x => x.Subject).ToList();
            }
            return courseList;
        }

        /// <summary>
        /// Filters list of all courses by selected grade level
        /// </summary>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses by selected grade</returns>
        public List<Course> GetCoursesByGrade(string gradeFilter)
        {
            List<Course> courseListByGrade;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListByGrade = context.Courses.Where(x => x.GradeLevel == gradeFilter).Include(x => x.User).OrderBy(x => x.Subject).ToList();
            }
            return courseListByGrade;
        }

        /// <summary>
        /// Filters list of course by selected course subject
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <returns>A list of courses by selected subject</returns>
        public List<Course> GetCoursesBySubject(string subjectFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter).Include(x => x.User).OrderBy(x => x.Subject).ToList();
            }
            return courseListBySubject;
        }

        /// <summary>
        /// Filters list of courses by selected course grade level and subject
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses by selected grade level and subject</returns>
        public List<Course> GetCoursesByGradeAndSubject(string subjectFilter, string gradeFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter && x.GradeLevel == gradeFilter).Include(x => x.User).OrderBy(x => x.Subject).ToList();
            }
            return courseListBySubject;
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
                    exception.ValidationExceptions.Add(new Exception("Error: Course ID does not exist"));
                    throw exception;
                }
                else
                {
                    Course course = context.Courses.Where(x => x.ID == courseID).Include(x => x.User).SingleOrDefault();

                    return course;
                }
            }
        }

        /* ------------------------------------------Administrator Instructor Actions -----------------------------------------------------*/

        /// <summary>
        /// Populates InstructorDetail view based on instructorID
        /// </summary>
        /// <param name="instructorID"></param>
        /// <returns>Instructor details on InstructorDetail View</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult InstructorDetail(int instructorID)
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                User instructor = GetInstructorByID(instructorID);
                ViewBag.Instructor = instructor;
                ViewBag.InstructorCourses = GetCoursesByInstructorID(instructor.ID);
            }
            catch(ValidationException e)
            {
                ViewBag.Message = "There is an issue(s) with the submission, please see the following details:";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }
            return View();
        }

        /// <summary>
        /// Populates a list of instructors on InstructorList View
        /// </summary>
        /// <returns>A list of instructors in InstructorList View</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult InstructorList()
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            ViewBag.Instructors = GetInstructors();

            return View();
        }

        /* -----------------------------------Administrator Instructor Data -----------------------------------------------------*/

        /// <summary>
        /// Uses LINQ quieries to populate a list of instructors based on the users role. In this case role of instructors is set to an identifier of 2.
        /// </summary>
        /// <returns>A list of instructors</returns>
        public List<User> GetInstructors()
        {
            List<User> instructorList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                instructorList = context.Users.Where(x => x.Role == 2).OrderBy(x => x.FirstName).ToList();
            }
            return instructorList;
        }

        /// <summary>
        /// Uses LINQ queries to get an instructor based on the user ID.
        /// </summary>
        /// <param name="instructorID"></param>
        /// <returns>An instructor based on user id</returns>
        public User GetInstructorByID(int instructorID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if(!context.Users.Any(x => x.ID == instructorID))
                {
                    exception.ValidationExceptions.Add(new Exception("Error: Cannot find instructor"));
                    throw exception;
                }

                User instructor = context.Users.Where(x => x.ID == instructorID).Include(x => x.Courses).SingleOrDefault();
                return instructor;
            }
        }

        /// <summary>
        /// Uses LINQ queries to return a list of courses that a particular instructor, based on user id, that the instructor is teaching.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of courses an instructor is teaching</returns>
        public List<Course> GetCoursesByInstructorID(int userID)
        {
            List<Course> teachingCourses;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                teachingCourses = context.Courses.Where(x => x.UserID == userID).Include(x => x.User).OrderBy(x => x.Subject).ToList();

                return teachingCourses;
            }
        }

        /* ---------------------------------Administrator Student Actions -----------------------------------------------------*/

        /// <summary>
        /// Populates StudentDetail view based on studentID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns>Student details on StudentDetail View</returns>
        public IActionResult StudentDetail(int studentID)
        {
            try
            {
                ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
                User student = GetStudentByID(studentID);
                ViewBag.Student = student;
                ViewBag.StudentCourses = GetEnrolledCoursesByStudentID(student.ID);

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
        /// Populates StudentList view with a list of students
        /// </summary>
        /// <returns>A list of students in the StudentList View</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult StudentList()
        {
            ViewBag.UserInformation = SharedFunctionsController.GetUserNameBySignInID(User.Identity.Name);
            ViewBag.Students = GetStudents();

            return View();
        }

        /* ------------------------------------Administrator Student Data -----------------------------------------------------*/
        
        /// <summary>
        /// Uses LINQ queries to find a given student by their user id
        /// If student not found, add exception to ValidationExceptions list
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns>A student based on user id</returns>
        public User GetStudentByID(int studentID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (!context.Users.Any(x => x.ID == studentID))
                {
                    exception.ValidationExceptions.Add(new Exception("Error: Cannot find Student"));
                    throw exception;
                }

                User student = context.Users.Where(x => x.ID == studentID).Include(x => x.Students).SingleOrDefault();
                return student;
            }
        }

        /// <summary>
        /// Uses LINQ queiries to aet a list of courses that a given student, based on the students user id, is enrolled in
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of enrolled courses per given student</returns>
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
                enrolledCourses = context.Courses.Where(x => courseIDs.Contains(x.ID)).Include(x => x.User).OrderBy(x => x.Subject).ToList();
                return enrolledCourses;
            }
        }
        /// <summary>
        /// Using LINQ queries to add students to a list of students
        /// </summary>
        /// <returns>A lsit of students</returns>
        public List<User> GetStudents()
        {
            List<User> studentList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                studentList = context.Users.Where(x => x.Role == 3).Include(x => x.Students).OrderBy(x => x.FirstName).ToList();
            }
            return studentList;
        }
    }
}
