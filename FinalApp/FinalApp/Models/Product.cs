using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public Category Category { get; set; }

        [Required(ErrorMessage = "Product must have a category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product must have a name.")]
        public string ProductName { get; set; }
        
        [DataType(DataType.Date)]
        [PastPurchaseDate(ErrorMessage = "Purchase date must be past or current date.")]
        public DateTime PurchaseDate { get; set; }
        
        [DataType(DataType.Date)]
        [FutureExpirationDate(ErrorMessage = "Expiration date must be in the future.")]
        public DateTime ExpirationDate { get; set; }

        public string UserName { get; set; }

        public class PastPurchaseDate : ValidationAttribute
        {
            public override bool IsValid(object date)
            {
                if (date == null) return true;
                DateTime purchaseDate = (DateTime)date;
                return purchaseDate < DateTime.Now;
            }
        }
        public class FutureExpirationDate : ValidationAttribute
        {
            public override bool IsValid(object date)
            {
                DateTime expirationDate = (DateTime)date;
                return expirationDate > DateTime.Now;
            }
        }
    }
}

