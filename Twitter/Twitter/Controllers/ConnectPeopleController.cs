using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            var followUsers = followService.GetActive();
            foreach (var item in followUsers)
            {
                users.Remove(userService.GetById(item.FromUserId));
            }
            return View(users);
        }

        public async Task<IActionResult> FollowUser(Guid id)
        {
            var connect = new FollowUser();
            connect.FromUserId = Guid.Parse(HttpContext.Session.GetString("ID"));
            connect.ToUserId = id;
            followService.Add(connect);
            return View();
        }
    }
}
