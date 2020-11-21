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
    public class AdministratorController : Controller
    {
        ValidationException exception = new ValidationException();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult List()
        {
            ViewBag.Courses = GetCourses();

            return View();
        }

        public IActionResult Delete(string courseID)
        {
            try
            {
                DeleteCourseByID(courseID);
            }
            catch (ValidationException e)
            {
                ViewBag.Courses = GetCourseByID(courseID);
                ViewBag.Message = "Could not delete course, there exist problem(s) with your submission, see below.";
                ViewBag.Exception = e;
                ViewBag.Error = true;

                return View("Details", new Dictionary<string, string>() { { "courseID", courseID } });
            }

            return RedirectToAction("List");
        }

        public void CreateCourse(string userID, string courseName, string subject, string courseDescription, string gradeLevel, DateTime startDate, DateTime endDate, int currentCapacity, int maxCapacity)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                context.Courses.Add(new Course()
                {
                    UserID = int.Parse(userID),
                    CourseName = courseName,
                    Subject = subject,
                    CourseDescription = courseDescription,
                    GradeLevel = gradeLevel,
                    StartDate = startDate,
                    EndDate = endDate,
                    CurrentCapacity = currentCapacity,
                    MaxCapacity = maxCapacity
                });
                context.SaveChanges();
            }
        }

        public List<Course> GetCourses()
        {
            List<Course> courseList;
            using (LearningManagementContext context = new LearningManagementContext())
            {
                courseList = context.Courses.Include(x => x.User).ToList();
            }
            return courseList;
        }

        public Course GetCourseByID(string courseID)
        {
            int parsedCourseID = int.Parse(courseID);
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (!context.Courses.Any(x => x.ID == int.Parse(courseID)))
                {
                    exception.ValidationExceptions.Add(new Exception("That Course ID Does Not Exist"));
                    throw exception;
                }
                else
                {
                    Course course = context.Courses.Where(x => x.ID == int.Parse(courseID)).Include(x => x.User).SingleOrDefault();

                    return course;
                }

            }
        }

        public void DeleteCourseByID(string courseID)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                context.Remove(GetCourseByID(courseID));
                context.SaveChanges();
            }
        }
    }
}
