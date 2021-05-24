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
        private readonly ICoreService<FollowUser> followUserService;
        private readonly IWebHostEnvironment env;

        public MainController(ICoreService<Tweet> tweewService, ICoreService<User> userService, ICoreService<FollowUser> followUserService, IWebHostEnvironment env)
        {
            this.tweewService = tweewService;
            this.userService = userService;
            this.followUserService = followUserService;
            this.env = env;
        }

        public IActionResult Index()
        {
            List<Tweet> tweets = tweewService.GetActive();
            tweets.Reverse();
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

                tweewService.Add(tweet);
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

        public IActionResult TweetLike(Guid id)
        {
            Tweet tweet = tweewService.GetById(id);
            tweet.LikeCount++;
            tweewService.Update(tweet);
            return RedirectToAction("Index", "Main");
        }

        public IActionResult TweetRetweet(Guid id)
        {
            if (tweewService.Any(x=> x.ID == id && x.UserID != Guid.Parse(HttpContext.Session.GetString("ID"))))
            {
                Tweet tweet = tweewService.GetById(id);
                Tweet reTweet = new Tweet
                {
                    TweetDetail = tweet.TweetDetail,
                    Tags = tweet.Tags,
                    UserID = Guid.Parse(HttpContext.Session.GetString("ID")),
                    RetweetCount = 0,
                    LikeCount = 0
                };
                tweet.RetweetCount++;
                tweewService.Update(tweet);
                tweewService.Add(reTweet);
            }
            return RedirectToAction("Index", "Main");
        }
    }
}
