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
        private readonly ICoreService<FollowUser> followService;

        public ProfileController(ICoreService<Tweet> tweewService, ICoreService<User> userService, ICoreService<FollowUser> followService)
        {
            this.tweewService = tweewService;
            this.userService = userService;
            this.followService = followService;
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
            var followFrom = followService.GetDefault(f => f.FromUserId == userId).Count;
            var followTo = followService.GetDefault(f => f.ToUserId == userId).Count;
            return View(Tuple.Create<User, List<Tweet>, int, int>(userService.GetById(userId), tweets, followFrom, followTo));
        }


        public IActionResult BackHome()
        {
            return RedirectToAction("Index", "Main");
        }

        public IActionResult FollowFrom()
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("ID"));
            var followfromlist = followService.GetDefault(f => f.FromUserId == userId);
            var users = new List<User>();
            foreach (var item in followfromlist)
            {
                users.Add(userService.GetByDefault(u => u.ID == item.FromUserId));
            }

            return View(users);
        }

        public IActionResult FollowTo()
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("ID"));
            var followtolist = followService.GetDefault(f => f.ToUserId == userId);
            var users = new List<User>();
            foreach (var item in followtolist)
            {
                users.Add(userService.GetByDefault(u => u.ID == item.ToUserId));
            }

            return View(users);
        }
    }
}
