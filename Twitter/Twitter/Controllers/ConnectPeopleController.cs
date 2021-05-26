using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;


namespace Twitter.Controllers
{
    public class ConnectPeopleController : Controller
    {
        private readonly ICoreService<User> userService;
        private readonly ICoreService<FollowUser> followService;

        public ConnectPeopleController(ICoreService<User> userService, ICoreService<FollowUser> followService)
        {
            this.userService = userService;
            this.followService = followService;
        }

        public IActionResult Index()
        {
            var users = userService.GetDefault(u => u.ID != Guid.Parse(HttpContext.Session.GetString("ID")) && u.Status == Core.Entity.Enum.Status.Active && u.Title != "Admin");
            var followUsers = followService.GetAll();
            foreach (var item in followUsers)
            {
                users.Remove(userService.GetById(item.ToUserId));
            }
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> FollowUser(Guid id)
        {
            var connect = new FollowUser();
            connect.FromUserId = Guid.Parse(HttpContext.Session.GetString("ID"));
            connect.ToUserId = id;
            if (followService.Any(x=> x.ToUserId != id))
            {
                bool term = followService.Add(connect);
            }
            return RedirectToAction("Index");
        }

        public IActionResult MoreThen()
        {
            return RedirectToAction("Index", "ConnectPeople");
        }
    }
}
