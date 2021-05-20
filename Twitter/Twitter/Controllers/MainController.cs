using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;

namespace Twitter.Controllers
{
    public class MainController : Controller
    {
        private readonly ICoreService<Tweet> tweewService;
        private readonly IWebHostEnvironment env;

        public MainController(ICoreService<Tweet> tweewService, IWebHostEnvironment env)
        {
            this.tweewService = tweewService;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }


        
    }
}
