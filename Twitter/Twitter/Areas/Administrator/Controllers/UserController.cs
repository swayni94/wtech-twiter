using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Entity.Enum;
using Twitter.Core.Service;
using Twitter.Model.Entities;

namespace Twitter.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize]
    public class UserController : Controller
    {
        private readonly ICoreService<User> userService;

        public UserController(ICoreService<User> userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            List<User> users = userService.GetDefault(x => x.Title != "Admin");
            return View(users);
        }

        public IActionResult Delete(Guid id)
        {
            userService.Remove(userService.GetById(id));
            return RedirectToAction("Index");
        }

        public IActionResult Activate(Guid id)
        {
            userService.Activate(id);
            return RedirectToAction("Index");
        }
        public IActionResult Ban(Guid id)
        {
            User user = userService.GetById(id);
            user.Status = Status.Banned;
            bool result = userService.Update(user);
            return RedirectToAction("Index");
        }
    }
}