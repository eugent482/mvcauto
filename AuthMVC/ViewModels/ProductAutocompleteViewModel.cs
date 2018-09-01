using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthMVC.ViewModels
{
    public class ProductAutocompleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
}