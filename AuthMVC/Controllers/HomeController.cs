using AuthMVC.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            //int mycount = _userService.GetCountUsers();
            //ViewBag.RoleId = _userService.AddRole("Moderator");
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Message = "No premission!!!";

            return View();
        }

    }
}