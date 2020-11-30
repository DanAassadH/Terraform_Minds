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

        public IActionResult Register()
        {
            return View();
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
            string gradeFilterUpper = gradeFilter.ToUpper();
            List<Course> courseListByGrade;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListByGrade = context.Courses.Where(x => x.GradeLevel == gradeFilter).Include(x => x.User).ToList();
            }
            return courseListByGrade;
        }

        public List<Course> GetCoursesBySubject(string subjectFilter)
        {
            string subjectFilterUpper = subjectFilter.ToUpper();
            List<Course> courseListBySubject;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseListBySubject = context.Courses.Where(x => x.Subject == subjectFilter).Include(x => x.User).ToList();
            }
            return courseListBySubject;
        }

        public List<Course> GetCoursesByGradeAndSubject(string subjectFilter, string gradeFilter)
        {
            string subjectFilterUpper = subjectFilter.ToUpper();
            string gradeFilterUpper = gradeFilter.ToUpper();
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

        public void RegisterCourse(int studentID, int courseID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                
            }
        }
    }
}
