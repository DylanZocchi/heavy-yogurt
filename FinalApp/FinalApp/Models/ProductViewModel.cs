using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinalApp.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

        public ProductViewModel(Product product, List<Category> categories)
        {
            Product = product;
            Categories = categories;
        }
    }

}
