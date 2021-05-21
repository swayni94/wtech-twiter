using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ICoreService<User> userService, IWebHostEnvironment env)
        {
            this.userService = userService;
            this.env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

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

                HttpContext.Session.SetString("ID", logged.ID.ToString());
                HttpContext.Session.SetString("Name", logged.FirstName);
                HttpContext.Session.SetString("Surname", logged.LastName);
                HttpContext.Session.SetString("Email", logged.EmailAddress);
                HttpContext.Session.SetString("Image", logged.ImagePath);

                var claims = new List<Claim>()
                {
                new Claim("ID", logged.ID.ToString()),
                new Claim(ClaimTypes.Name, logged.FirstName),
                new Claim(ClaimTypes.Surname, logged.LastName),
                new Claim(ClaimTypes.Email, logged.EmailAddress),
                new Claim("Image", logged.ImagePath)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

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
