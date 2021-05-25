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

        public ConnectPeopleViewComponent(ICoreService<User> userService)
        {
            this.userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            return View(userService.GetDefault(u => u.ID != Guid.Parse(HttpContext.Session.GetString("ID")) && u.Status == Core.Entity.Enum.Status.Active && u.Title != "Admin").Take(4).ToList());
        }
    }
}
