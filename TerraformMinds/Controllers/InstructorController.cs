using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TerraformMinds.Controllers
{
    public class InstructorController : Controller
    {
        [Authorize(Roles = "Instructor")]
        public IActionResult Index()
        {
            return RedirectToAction("CourseList");

        }

        public IActionResult CourseList()
        {

            return View();
        }
    }
}
