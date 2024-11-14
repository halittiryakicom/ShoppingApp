﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.WebApi.Models
{
    public class AddProductRequest
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}
