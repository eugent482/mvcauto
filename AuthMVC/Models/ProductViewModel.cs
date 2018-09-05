using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthMVC.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(50), Display(Name = "Product name")]
        public string Name { get; set; }
        [StringLength(200), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public string Photo { get; set; }
    }
}