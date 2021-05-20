using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitter.Core.Entity.Enum;
using Twitter.Core.Service;
using Twitter.Model.Entities;
using Twitter.Models;

namespace Twitter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoreService<User> userService;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICoreService<User> userService, IWebHostEnvironment env)
        {
            this.userService = userService;
            this.env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        /*
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (userService.Any(x => x.EmailAddress == user.EmailAddress && x.Password == user.Password && x.Status == Status.Active && x.Title != "Admin"))
            {
                User logged = userService.GetByDefault(x => x.EmailAddress == user.EmailAddress && x.Password == user.Password);

                var claims = new List<Claim>()
                {
                new Claim("ID", logged.ID.ToString()),
                new Claim(ClaimTypes.Name, logged.FirstName),
                new Claim(ClaimTypes.Surname, logged.LastName),
                new Claim(ClaimTypes.Email, logged.EmailAddress),
                new Claim("Image", logged.ImagePath)
                };

                //Giriş işlemleri ve ardından yönetici sayfasına yönlendireceğiz
                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var email = userService.GetByDefault(x => x.EmailAddress == user.EmailAddress);
                if (email == null)
                {
                    if (files.Count > 0)
                    {
                        Upload upload = new Upload();
                        bool imgResult;
                        string imgPath = upload.ImageUpload(files, env, out imgResult);
                        if (imgResult)
                        {
                            user.ImagePath = imgPath;
                        }
                        else
                        {
                            ViewBag.Message = imgPath;
                            return View();
                        }
                    }
                    user.Title = "User";
                    bool result = userService.Add(user);
                    if (result)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["Message"] = $"Kayıt işleminde bir hata oluştu. Lütfen tüm alanları kontrol edip tekrar deneyin.";
                    }
                }
                else
                {
                    TempData["Message"] = $"Bu Mail Adresi zaten kullanılmaktadır.";
                }
            }
            else
            {
                TempData["Message"] = $"İşlem başarısız oldu. 1023241 hata koduyla başvurun.";
            }

            return View(user);
        }
    }
}
