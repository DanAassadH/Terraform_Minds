using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Controllers
{
    public class SignInController : Controller
    {
        /* ----------------------------------------------- Actions ----------------------------------------*/
        public IActionResult Index()
        {
            return RedirectToAction("SignIn");

        }

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
