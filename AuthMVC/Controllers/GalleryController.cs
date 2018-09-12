using AuthMVC.Models;
using AuthMVC.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthMVC.Controllers
{
    public class GalleryController : Controller
    {

        private ApplicationDbContext db;// = new ApplicationDbContext();

        public GalleryController(ApplicationDbContext _db)
        {
            db = _db;
        }

        // GET: Gallery
        public ActionResult Index()
        {
            var list = db.GalleryPhotos.ToList();


            return View(list);
        }

        [HttpPost]
        public ActionResult AddPhoto(string image)
        {
            PhotoGallery photo;
            if (string.IsNullOrEmpty(image))
                return RedirectToAction("Error", "Home");
            try
            {
                int startIndex = image.IndexOf("/") + 1;
                int lastIndex = image.IndexOf(";");
                var extension = "." + image.Substring(startIndex, lastIndex - startIndex);
                var filename = Guid.NewGuid().ToString();
                var path = "/Content/GalleryMain/" + filename + extension;
                var fs = new BinaryWriter(new FileStream(Server.MapPath(path), FileMode.Create, FileAccess.Write));
                string base64img = image.Split(',')[1];
                byte[] buf = Convert.FromBase64String(base64img);
                fs.Write(buf);
                fs.Close();
                photo = new PhotoGallery
                {
                    MainPhoto = path,
                    PhotoPreview="nopreview"
                };
                db.GalleryPhotos.Add(photo);
                db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }  
            string json = JsonConvert.SerializeObject(photo);
            return Content(json, "application/json");
        }


        [HttpPost]
        public ActionResult DeletePhoto(int id)
        {
                      
            try
            {
                var photo=db.GalleryPhotos.Find(id);

                if (photo != null)
                {
                    db.GalleryPhotos.Remove(photo);
                    db.SaveChanges();
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
            string json = JsonConvert.SerializeObject(id);
            return Content(json, "application/json");
        }

    }
}