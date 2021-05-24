using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;

namespace Twitter.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ICoreService<Tweet> tweewService;
        private readonly ICoreService<User> userService;

        public ProfileController(ICoreService<Tweet> tweewService, ICoreService<User> userService)
        {
            this.tweewService = tweewService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("ID"));
            List<Tweet> tweets = tweewService.GetDefault(x => x.UserID == userId);
            tweets.Reverse();
            foreach (var item in tweets)
            {
                item.User = userService.GetById(item.UserID);
            }

            return View(Tuple.Create<User, List<Tweet>>(userService.GetById(userId), tweets));
        }


        public IActionResult BackHome()
        {
            return RedirectToAction("Index", "Main");
        }
    }
}
