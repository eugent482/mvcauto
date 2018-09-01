using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthMVC.Models.Entity
{
    [Table("tblCategories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Category name"),Required(ErrorMessage ="The category  name cannot be blank"),StringLength (50,MinimumLength =3,ErrorMessage ="Size must be between 3 and 50")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}