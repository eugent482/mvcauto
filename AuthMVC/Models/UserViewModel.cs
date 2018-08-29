using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthMVC.Models
{
    public class UserViewModel
    {
        [Display(Name = "User id")]
        public int UserID { get; set; }
        [Display(Name = "User login")]
        public string UserLogin { get; set; }
        [Display(Name = "User country")]
        public string Country { get; set; }
        [Display(Name = "User role")]
        public string UserRole { get; set; }
    }
}