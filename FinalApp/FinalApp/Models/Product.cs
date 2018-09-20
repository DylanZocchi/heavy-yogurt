﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalApp.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        [Required]
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public string ProductName { get; set; }

        //Purchase Date
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        //Expiration Date
        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public string UserName { get; set; }

    }
}
