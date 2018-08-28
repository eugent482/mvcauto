using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthMVC.Controllers
{
    [MyCustomAuthorize(Roles = "Administrator")]
    public class EditController : Controller
    {
        // GET: Edit
        public ActionResult Index()
        {
            return View();
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