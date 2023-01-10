using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcTestApp.Models
{
    public class ProductListModel
    {
        public CategoryModel Category { get; set; }
        public ProductModel Product { get; set; }
    }
}