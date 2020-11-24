using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TerraformMinds.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult RegisterUser()
        {
            return View();
        }
    }
}
