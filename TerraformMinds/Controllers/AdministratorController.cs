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

        //public void CreateCourse(string userID, string courseName, string subject, string courseDescription, string gradeLevel, DateTime startDate, DateTime endDate, int currentCapacity, int maxCapacity)
        //{
        //    using (LearningManagementContext context = new LearningManagementContext())
        //    {
        //        context.Courses.Add(new Course()
        //        {
        //            UserID = int.Parse(userID),
        //            CourseName = courseName,
        //            Subject = subject,
        //            CourseDescription = courseDescription,
        //            GradeLevel = gradeLevel,
        //            StartDate = startDate,
        //            EndDate = endDate,
        //            CurrentCapacity = currentCapacity,
        //            MaxCapacity = maxCapacity
        //        });
        //        context.SaveChanges();
        //    }
        //}

        // POST: Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,CurrentCapacity,MaxCapacity")] Course course)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                if (ModelState.IsValid)
                {
                    context.Add(course);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
            }
            return View(course);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,CourseName,Subject,CourseDescription,GradeLevel,StartDate,EndDate,CurrentCapacity,MaxCapacity")] Course course)
        {
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
                        context.Update(course);
                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!(context.Courses.Any(x => x.ID == id)))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(List));
                }
                return View(course);
            }
        }

        public async Task<IActionResult> Edit(int? id)
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

                var instructors = new SelectList(context.Users.Where(x => x.Role == 2)
                    .OrderBy(y => y.LastName)
                    .ToDictionary(us => us.ID, us => us.FirstName + " " + us.LastName), "Key", "Value", course.UserID);
                ViewBag.Instructors = instructors;
                ViewBag.CourseName = course.CourseName;
                ViewBag.Subject = course.Subject;
                ViewBag.StartDate = course.StartDate;
                ViewBag.EndDate = course.EndDate;
                ViewBag.CourseDescription = course.CourseDescription;

                // Create list if GradeLevels 
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

        public async Task<IActionResult> Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (LearningManagementContext context = new LearningManagementContext())
            {
                var course = await context.Courses.FindAsync(id);
                course.UserID = null;
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
        }
    }
}
