using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core.Service;
using Twitter.Model.Entities;
using Twitter.Models;

namespace Twitter.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize]
    public class TweetController : Controller
    {
        private readonly ICoreService<Tweet> tweetService;
        private readonly IWebHostEnvironment env;

        public TweetController(ICoreService<Tweet> tweetService, IWebHostEnvironment env)
        {
            this.tweetService = tweetService;
            this.env = env;
        }

        public IActionResult Index()
        {
            return View(tweetService.GetAll());
        }

        public IActionResult Delete(Guid id)
        {
            tweetService.Remove(tweetService.GetById(id));
            return RedirectToAction("Index");
        }

        public IActionResult Activate(Guid id)
        {
            tweetService.Activate(id);
            return RedirectToAction("Index");
        }

        public IActionResult Update(Guid id)
        {
            return View(tweetService.GetById(id));
        }


        [HttpPost]
        public IActionResult Update(Tweet item, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                Tweet updated = tweetService.GetById(item.ID);
                updated.TweetDetail = item.TweetDetail;
                updated.ImagePath = item.ImagePath;
                updated.Tags = item.Tags;

                if (files.Count > 0)
                {
                    bool imgResult;
                    Upload upload = new Upload();
                    string imgPath = upload.ImageUpload(files, env, out imgResult);
                    if (imgResult)
                    {
                        updated.ImagePath = imgPath;
                        bool result = tweetService.Update(updated);
                        if (result)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Hata oluştu.";
                        }
                    }
                    else
                    {
                        TempData["Message"] = imgPath;
                        return View(item);
                    }
                }
            }
            else
            {
                TempData["Message"] = "İşlem başarısız oldu. Lütfen tüm alanları kontrol ediniz.";
            }
            return View(item);
        }
    }
}
