using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TerraformMinds.Controllers
{
    public class RolecheckController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }
        [Authorize(Roles = "Instructor")]
        public IActionResult Instructor()
        {
            return View();
        }


    }
}
