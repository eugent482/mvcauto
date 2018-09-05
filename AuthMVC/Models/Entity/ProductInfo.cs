using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthMVC.Models.Entity
{
    [Table("tblProductInfo")]
    public class ProductInfo
    {
        [Key]
        [ForeignKey("Product")]
        public int Id { get; set; }
        public string Photo { get; set; }
        public virtual Product Product { get; set; }
    }
}