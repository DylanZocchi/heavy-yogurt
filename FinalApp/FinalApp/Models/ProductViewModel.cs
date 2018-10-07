using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinalApp.Models
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<Category> Categories { get; set; }

        public ProductViewModel(Product product, List<Category> category)
        {
            this.product = product;
            this.Categories = category;

        }
    }

}
