using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Twitter.Models
{
    public class Upload
    {
        public string ImageUpload(List<IFormFile> files, IWebHostEnvironment environment, out bool imgResult)
        {
            imgResult = false;
            var uploads = Path.Combine(environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))
                {
                    if (file.Length <= 2097152)
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split('/')[1]}";

                        var filePath = Path.Combine(uploads, uniqueName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            imgResult = true;
                            return filePath.Substring(filePath.IndexOf("uploads"));
                        }
                    }
                    else
                    {
                        return $"2 MB'den büyük resim yükleyemezsiniz.";
                    }
                }
                else
                {
                    return $"Lütfen sadece resim dosyası yükleyin.";
                }
            }
            return "Dosya bulunamadı! Lütfen en az 1 dosya seçiniz.";
        }
    }
}
