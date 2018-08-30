using AuthMVC.DAL.Abstract;
using AuthMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuthMVC.Controllers
{
    [MyCustomAuthorize(Roles = "Administrator, Moderator")]
    public class EditController : Controller
    {
        private readonly IUserService _userService;

        private ApplicationUserManager _userManager;

        public EditController(IUserService userService, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _userService = userService;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager;// ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        // GET: Edit
        public ActionResult Index()
        {
            var items = _userService.GetUsers();

            return View(items);
        }       
              
       [HttpPost]
        public ActionResult Delete(int id)
        {
            var uid = Int32.Parse(User.Identity.GetUserId());
            if (_userService.CanManage(uid, id))
            {
                _userService.DeleteUser(id);


                return RedirectToAction("Index", "Edit");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(UserEditModel model)
        {
            if (ModelState.IsValid)
            {

                if (_userService.GetRoleId(Int32.Parse(User.Identity.GetUserId())) < model.Role)
                {
                    string path = "";
                    string filename = "";
                    string extension = "";
                    if (model.Photo != null)
                    {
                        int startIndex = model.Photo.IndexOf("/") + 1;
                        int lastIndex = model.Photo.IndexOf(";");
                        extension = "." + model.Photo.Substring(startIndex, lastIndex - startIndex);
                        filename = model.Email + "_avatar";
                        path = @"/Content/SaveAvatars/" + filename + extension;
                    }

                    var result = _userService.ChangeUser(model);

                    if (result)
                    {

                        return RedirectToAction("Index", "Edit");
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }
                   
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            ViewBag.Roles = _userService.GetRoles().Where(x => x.Id > _userService.GetRoleId(Int32.Parse(User.Identity.GetUserId())));
            return View(model);
        }



        [HttpGet]
        public ActionResult Change(int id)
        {
            var uid = Int32.Parse(User.Identity.GetUserId());
            if (_userService.CanManage(uid, id))
            {
                var usermodel = _userService.GetUser(id);
                var roles = _userService.GetRoles().Where(x => x.Id > _userService.GetRoleId(Int32.Parse(User.Identity.GetUserId())));
                roles.Reverse();
                ViewBag.Roles = roles;
                return View(usermodel);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult Register()
        {
            var roles = _userService.GetRoles().Where(x=>x.Id> _userService.GetRoleId(Int32.Parse(User.Identity.GetUserId())));
            roles.Reverse();

            ViewBag.Roles = roles;

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model,int roles)
        {
            if (ModelState.IsValid)
            {
               
                if (_userService.GetRoleId(Int32.Parse(User.Identity.GetUserId()))<roles)
                {

                    string path = "";
                    string filename = "";
                    string extension = "";
                    if (model.Photo != null)
                    {
                        int startIndex = model.Photo.IndexOf("/") + 1;
                        int lastIndex = model.Photo.IndexOf(";");
                        extension = "." + model.Photo.Substring(startIndex, lastIndex - startIndex);
                        filename = model.Email + "_avatar";
                        path = @"/Content/SaveAvatars/" + filename + extension;
                    }
                    CustomUserRole role = new CustomUserRole { RoleId = roles };
                    var profile = new UserProfile { Address = model.Address, Photo = path, BirthDay = Convert.ToDateTime(model.BirthDay) };
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Profile = profile };
                    user.Roles.Add(role);
                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (model.Photo != null)
                        {

                            var fs = new BinaryWriter(new FileStream(Server.MapPath("~/Content/SaveAvatars/" + filename + extension), FileMode.Create, FileAccess.Write));
                            string base64img = model.Photo.Split(',')[1];
                            byte[] buf = Convert.FromBase64String(base64img);
                            fs.Write(buf);
                            fs.Close();

                        }

                        return RedirectToAction("Index", "Edit");
                    }
                    AddErrors(result);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            ViewBag.Roles = _userService.GetRoles().Where(x => x.Id > _userService.GetRoleId(Int32.Parse(User.Identity.GetUserId())));
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


    }



    public class MyCustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
            }
            else if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Home/Error");
            }
        }
    }
}