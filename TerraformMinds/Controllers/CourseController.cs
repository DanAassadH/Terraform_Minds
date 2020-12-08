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
    public class CourseController : Controller
    {
        ValidationException exception = new ValidationException();

        /// <summary>
        /// Goes to Index View
        /// </summary>
        /// <returns>Index View</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Populates a list of courses in CourseList View
        /// Course List view can change based on filters selected.
        /// Course List can be filtered by Grade Level, Subject or both
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses in CourseList View</returns>
        public IActionResult CourseList(string gradeFilter, string subjectFilter)
        {
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

            else if(courseSubjects.Contains(subjectFilter) && gradeLevels.Contains(gradeFilter))
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
        /// Populates CourseDetail View based on courseID
        /// If errors were found show erros in CourseDetail View
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns>CourseDetail View</returns>
        public IActionResult CourseDetail(int courseID)
        {
            try
            {
                ViewBag.CourseDetails = GetCourseByID(courseID); 
            }
            catch(ValidationException e)
            {
                ViewBag.Message = "There Exist problem(s) with your submission, see below";
                ViewBag.Exception = e;
                ViewBag.Error = true;
            }
            return View();
        }

        /// <summary>
        /// Populates Course detail, with enroll function, in CourseDetail View
        /// If errors are found, show errors in CourseDetail View
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="userIdentity"></param>
        /// <returns>Course Detail, with enroll function, in CourseDetail View</returns>
        public IActionResult Enroll (int courseID, int userIdentity)
        {

            try
            {
                RegisterCourse(courseID, userIdentity);
                ViewBag.Message = $"Course successfully  registered";
                ViewBag.CourseDetails = GetCourseByID(courseID);
            }

            catch(ValidationException e)
            {
                ViewBag.Message = "There exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;
                ViewBag.CourseDetails = GetCourseByID(courseID);
            }

            return View("CourseDetail");
        }

        /// <summary>
        /// Add all courses from database into a list of courses
        /// </summary>
        /// <returns>A lsit of courses</returns>
        public List<Course> GetCoursesAll()
        {
            List<Course> courseList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseList = context.Courses.Include(x => x.User).ToList();
            }
            return courseList;
        }

        //public List<Course> GetCurrentCourses()
        //{
        //    List<Course> courseList;
        //    using (LearningManagementContext context = new LearningManagementContext())
        //    {
        //        courseList = context.Courses.Where(x => x.EndDate > DateTime.Today || x.EndDate == null).Include(x => x.User).ToList();
        //    }
        //    return courseList;
        //}

        /// <summary>
        /// Filter Setting for course list. Will filter the list of courses based on selected grade level
        /// </summary>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses based on selected grade level</returns>
        public List<Course> GetCoursesByGrade(string gradeFilter)
        {
            List<Course> courseListByGrade;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListByGrade = context.Courses.Where(x => x.GradeLevel == gradeFilter).Include(x => x.User).ToList();
            }
            return courseListByGrade;
        }

        /// <summary>
        /// Filter setting for course list. Will filter the list of courses based on selected course subject
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <returns>A list of courses based on selected subject</returns>
        public List<Course> GetCoursesBySubject(string subjectFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter).Include(x => x.User).ToList();
            }
            return courseListBySubject;
        }

        /// <summary>
        /// Filter setting for course list. Will filter the list of courses based on selected grade level and subject
        /// </summary>
        /// <param name="subjectFilter"></param>
        /// <param name="gradeFilter"></param>
        /// <returns>A list of courses based on selected grade level and subject</returns>
        public List<Course> GetCoursesByGradeAndSubject(string subjectFilter, string gradeFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter && x.GradeLevel == gradeFilter).Include(x => x.User).ToList();
            }
            return courseListBySubject;
        }

        /// <summary>
        /// Finds a course based on its course ID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns>A course based on its course ID</returns>
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
                    Course course = context.Courses.Where(x => x.ID == courseID).Include(x => x.User).Include(x => x.Students).SingleOrDefault();

                    return course;
                }
            }
        }

        /// <summary>
        /// Update command for when a student enrolls in a course based on the course ID and if the user is logged in under the correct role of student.
        /// 
        /// If there are validation exceptions, add them to validation exception list.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="userIndentity"></param>
        public void RegisterCourse(int courseID, int userIndentity)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                Course enrollCourse = context.Courses.Where(x => x.ID == courseID).SingleOrDefault();

                if(User.Identity.Name == null)
                {
                    exception.ValidationExceptions.Add(new Exception("Please sign-in to enroll"));
                    //throw exception;
                }
                else
                {
                    User userRoll = context.Users.Where(x => x.Role == 3 && x.ID == int.Parse(User.Identity.Name)).SingleOrDefault();

                    if (userRoll == null)
                    {
                        exception.ValidationExceptions.Add(new Exception("You must be a student to enroll"));
                        //throw exception;
                    }
                    else
                    {
                        if (context.Students.Any(x => x.CourseID == courseID && x.UserID == int.Parse(User.Identity.Name)))
                        {
                            exception.ValidationExceptions.Add(new Exception("You have already enrolled for this course."));
                            //throw exception;
                        }

                        else
                        {
                            if (enrollCourse.CurrentCapacity >= enrollCourse.MaxCapacity)
                            {
                                exception.ValidationExceptions.Add(new Exception("Sorry, the course you are registering for is already full"));
                                //throw exception;
                            }
                            else
                            {
                                if (enrollCourse.EndDate <= DateTime.Today)
                                {
                                    exception.ValidationExceptions.Add(new Exception("Sorry, that course has already ended."));
                                    //throw exception;
                                }
                            }
                        }
                    }
                }
                
                if (exception.ValidationExceptions.Count > 0)
                {
                    throw exception;
                }

                context.Students.Add(new Student()
                {
                    UserID = int.Parse(User.Identity.Name),
                    CourseID = courseID,
                });

                enrollCourse.CurrentCapacity += 1;

                context.SaveChanges();
            }
        }
    }
}
