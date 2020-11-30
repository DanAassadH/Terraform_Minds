using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Controllers
{
    public class StudentController : Controller
    {
        /* ------------------------------------------Actions -----------------------------------------------------*/
        [Authorize(Roles = "Student")]
        public IActionResult StudentDashboard()
        {
            // return RedirectToAction("CourseList");
            return View();

        }
    }
}
