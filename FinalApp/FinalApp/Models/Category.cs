using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name may not be blank.")]
        public string CategoryName { get; set; }
        
        public string UserName { get; set; }
    }
    
}
