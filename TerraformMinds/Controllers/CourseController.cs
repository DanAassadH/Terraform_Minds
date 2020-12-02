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

        public IActionResult Index()
        {
            return View();
        }

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

        public IActionResult Register (int courseID)
        {

            try
            {
                RegisterCourse(courseID);
                ViewBag.Message = $"Course sucessfully registered";
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

        
        public List<Course> GetCoursesAll()
        {
            List<Course> courseList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseList = context.Courses.Include(x => x.User).ToList();
            }
            return courseList;
        }

        public List<Course> GetCoursesByGrade(string gradeFilter)
        {
            List<Course> courseListByGrade;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListByGrade = context.Courses.Where(x => x.GradeLevel == gradeFilter).Include(x => x.User).ToList();
            }
            return courseListByGrade;
        }

        public List<Course> GetCoursesBySubject(string subjectFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter).Include(x => x.User).ToList();
            }
            return courseListBySubject;
        }

        public List<Course> GetCoursesByGradeAndSubject(string subjectFilter, string gradeFilter)
        {
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter && x.GradeLevel == gradeFilter).Include(x => x.User).ToList();
            }
            return courseListBySubject;
        }

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

        public void RegisterCourse(int courseID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                Course enrollCourse = context.Courses.Where(x => x.ID == courseID).SingleOrDefault();

                User userRoll = context.Users.Where(x => x.Role == 3 && x.ID == int.Parse(User.Identity.Name)).SingleOrDefault();

                if (userRoll == null)
                {
                    exception.ValidationExceptions.Add(new Exception("Please sign-in to enroll"));
                    throw exception;
                }
                else
                {
                    if(context.Students.Any(x => x.CourseID == courseID))
                    {
                        exception.ValidationExceptions.Add(new Exception("You have already enrolled for this course."));
                        throw exception;
                    }

                    else
                    {
                        if(enrollCourse.CurrentCapacity >= enrollCourse.MaxCapacity)
                        {
                            exception.ValidationExceptions.Add(new Exception("Sorry, the course you are registering for is already full"));
                            throw exception;
                        }
                        else
                        {
                            if(enrollCourse.StartDate < DateTime.Today)
                            {
                                exception.ValidationExceptions.Add(new Exception("Sorry, registration for that course is now closed"));
                                throw exception;
                            }

                            else
                            {
                                if(enrollCourse.EndDate <= DateTime.Today)
                                {
                                    exception.ValidationExceptions.Add(new Exception("Sorry, that course has already ended."));
                                    throw exception;
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

        //public void CourseCapacity(int courseID)
        //{

        //    using (LearningManagementContext context = new LearningManagementContext())
        //    {
        //        List<Student> courseCount = context.Students.Where(x => x.CourseID == courseID).ToList();
        //        Course courses = context.Courses.Where(x => x.ID == courseID).SingleOrDefault();

        //        int courseCurrentCapacity = courseCount.Count();

        //        courses.CurrentCapacity = courseCurrentCapacity;
                
        //    }
        //}
    }
}
