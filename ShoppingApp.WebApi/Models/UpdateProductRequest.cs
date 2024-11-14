using ShoppingApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.WebApi.Models
{
    public class UpdateProductRequest
    {
        [Required]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> OrderProductsId { get; set; }
    }
}
