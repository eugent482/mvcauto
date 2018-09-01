using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthMVC.Models.Entity
{
    [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50),Display(Name="Product name")]
        public string Name { get; set; }
        [StringLength(200), DataType(DataType.MultilineText)]
        public string Description { get; set; }        
        public decimal Price { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}