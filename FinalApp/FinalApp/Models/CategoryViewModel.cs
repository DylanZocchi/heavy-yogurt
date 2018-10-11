using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalApp.Models
{
    internal class CategoryViewModel : Category
    {
        public CategoryViewModel() : this(null)
        {

        }
        public CategoryViewModel(Category category)
        {
            CategoryId = category.CategoryId;
            CategoryName = category.CategoryName;
            UserName = category.UserName;
        }
    }
}
