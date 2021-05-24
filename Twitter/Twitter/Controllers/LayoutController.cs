using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Twitter.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            return RedirectToAction("Index", "Main");
        }

        public IActionResult Profile()
        {
            return RedirectToAction("Index", "Profile");
        }
    }
}
