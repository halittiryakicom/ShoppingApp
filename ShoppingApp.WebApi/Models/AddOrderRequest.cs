using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.WebApi.Models
{
    public class AddOrderRequest
    {
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
