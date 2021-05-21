using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;

namespace Twitter.Controllers
{
    public class MainController : Controller
    {
        private readonly ICoreService<Tweet> tweewService;
        private readonly ICoreService<User> userService;
        private readonly IWebHostEnvironment env;

        public MainController(ICoreService<Tweet> tweewService, ICoreService<User> userService, IWebHostEnvironment env)
        {
            this.tweewService = tweewService;
            this.userService = userService;
            this.env = env;
        }

        public IActionResult Index()
        {
            List<Tweet> tweets = tweewService.GetActive();
            foreach (var item in tweets)
            {
                item.User = userService.GetById(item.UserID);
            }

            return View(Tuple.Create<User, List<Tweet>>(userService.GetById(Guid.Parse(HttpContext.Session.GetString("ID"))), tweets));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTweet(Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                tweet.UserID = Guid.Parse(HttpContext.Session.GetString("ID"));
                //tweet.User = userService.GetById(tweet.UserID);
                tweet.RetweetCount = 0;
                tweet.LikeCount = 0;
                tweet.ImagePath = "";
                tweet.Tags = "";

                bool term = tweewService.Add(tweet);
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

        [HttpPost]
        public IActionResult TweetLike(Guid guid)
        {
            Tweet tweet = tweewService.GetById(guid);
            tweet.LikeCount++;
            tweewService.Update(tweet);
            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public IActionResult TweetRetweet(Guid guid)
        {
            if (tweewService.Any(x=> x.ID == guid && x.UserID != Guid.Parse(HttpContext.Session.GetString("ID"))))
            {
                Tweet tweet = tweewService.GetById(guid);
                User user = userService.GetById(Guid.Parse(HttpContext.Session.GetString("ID")));
                tweet.RetweetCount++;
                tweet.UserID = user.ID;
                tweewService.Add(tweet);
            }
            return RedirectToAction("Index", "Main");
        }
    }
}
