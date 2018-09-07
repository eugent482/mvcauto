using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuthMVC.Models;
using AuthMVC.Models.Entity;
using AuthMVC.ViewModels;
using Newtonsoft.Json;

namespace AuthMVC.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db;// = new ApplicationDbContext();

        public ProductsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [NoCache]
        // GET: Products
        public ActionResult Index(string search)
        {

            var products = db.Products.Include(p => p.Category).Include(x=>x.Info);



            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search)
                    || p.Description.Contains(search)
                    || p.Category.Name.Contains(search));
            }

            List<ProductViewModel> models = new List<ProductViewModel>();
          
                foreach (var item in products)
                {
                    ProductViewModel model = new ProductViewModel
                    {
                        Id = item.Id,
                        Name=item.Name,
                        CategoryId = item.CategoryId,
                        Description = item.Description,
                        Price = item.Price,                        
                        CategoryName = item.Category.Name

                    };
                if (item.Info != null)
                {
                    model.Photo = item.Info.Photo;
                }
                    models.Add(model);
                }
            
            return View(models);

        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public sealed class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }



        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name

            };
            if (product.Info != null)
            {
                model.Photo = product.Info.Photo;
            }

            return View(model);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel productview)
        {

            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Name = productview.Name,
                    Description = productview.Description,
                    Price = productview.Price,
                    CategoryId = productview.CategoryId   ,
                    Info=new ProductInfo()
                };
                db.Products.Add(product);
                db.SaveChanges();
                string path = "";
                string filename = "";
                string extension = "";
                if (!String.IsNullOrEmpty(productview.Photo))
                {
                    int startIndex = productview.Photo.IndexOf("/") + 1;
                    int lastIndex = productview.Photo.IndexOf(";");
                    extension = "." + productview.Photo.Substring(startIndex, lastIndex - startIndex);
                    filename = product.Id + "_avatar";
                    path = @"/Content/ProductAvatars/" + filename + extension;
                    product.Info.Photo = path;
                    db.SaveChanges();
                    var fs = new BinaryWriter(new FileStream(Server.MapPath("~/Content/ProductAvatars/" + filename + extension), FileMode.Create, FileAccess.Write));
                    string base64img = productview.Photo.Split(',')[1];
                    byte[] buf = Convert.FromBase64String(base64img);
                    fs.Write(buf);
                    fs.Close();
                }
               
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", productview.CategoryId);
            return View(productview);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name

            };
            if (product.Info != null)
            {
                model.Photo = product.Info.Photo;
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel productview)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Products.Find(productview.Id);
                product.Name = productview.Name;
                product.Description = productview.Description;
                product.Price = productview.Price;
                product.CategoryId = productview.CategoryId;
                

                string path = "";
                string filename = "";
                string extension = "";
                if (!String.IsNullOrEmpty(productview.Photo))
                {
                    if (product.Info == null)
                        product.Info = new ProductInfo();
                    if (!String.IsNullOrEmpty(product.Info.Photo))
                    {
                        try
                        {                            
                            System.IO.File.Delete(product.Info.Photo);
                        }
                        catch (Exception)
                        { }
                    }
                    int startIndex = productview.Photo.IndexOf("/") + 1;
                    int lastIndex = productview.Photo.IndexOf(";");
                    extension = "." + productview.Photo.Substring(startIndex, lastIndex - startIndex);
                    filename = product.Id + "_avatar";
                    path = @"/Content/ProductAvatars/" + filename + extension;
                    product.Info.Photo = path;
                }
                db.SaveChanges();
                if (!String.IsNullOrEmpty(productview.Photo))
                {
                    var fs = new BinaryWriter(new FileStream(Server.MapPath("~/Content/ProductAvatars/" + filename + extension), FileMode.Create, FileAccess.Write));
                    string base64img = productview.Photo.Split(',')[1];
                    byte[] buf = Convert.FromBase64String(base64img);
                    fs.Write(buf);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", productview.CategoryId);
            return View(productview);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name

            };
            if (product.Info != null)
            {
                model.Photo = product.Info.Photo;
            }
            return View(model);
        }

        #region
        [HttpGet]
        public ContentResult SearchByNameJson(string search)
        {
            var products = db.Products.Include(p => p.Category).Include(x=>x.Info).Where(p => p.Name.Contains(search)
                 || p.Description.Contains(search)
                 || p.Category.Name.Contains(search)).Select(p => new ProductAutocompleteViewModel
                 {
                     Id = p.Id,
                     Name = p.Name,
                     CategoryName = p.Category.Name,
                     Image=p.Info.Photo
                 }).ToList();

         

            string json = JsonConvert.SerializeObject(products);

            return Content(json, "application/json");

        }
        #endregion

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);    
            if(product.Info!=null)
                db.ProductInfoes.Remove(product.Info);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
