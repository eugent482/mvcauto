using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthMVC.Models
{
    [Table("tblUsers")]
    public class UserProfile
    {
        [ForeignKey("User")]
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}