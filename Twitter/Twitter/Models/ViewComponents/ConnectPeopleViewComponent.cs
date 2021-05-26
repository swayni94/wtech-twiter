using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;

namespace Twitter.Models.ViewComponents
{
    public class ConnectPeopleViewComponent : ViewComponent
    {
        private readonly ICoreService<User> userService;
        private readonly ICoreService<FollowUser> followService;

        public ConnectPeopleViewComponent(ICoreService<User> userService, ICoreService<FollowUser> followService)
        {
            this.userService = userService;
            this.followService = followService;
        }

        public IViewComponentResult Invoke()
        {
            var users = userService.GetDefault(u => u.ID != Guid.Parse(HttpContext.Session.GetString("ID")) && u.Status == Core.Entity.Enum.Status.Active && u.Title != "Admin");
            var followUsers = followService.GetAll();
            foreach (var item in followUsers)
            {
                users.Remove(userService.GetById(item.ToUserId));
            }
            return View(users);
        }
    }
}
