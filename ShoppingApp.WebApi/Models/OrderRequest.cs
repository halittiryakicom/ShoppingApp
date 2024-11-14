using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.WebApi.Models
{
    public class OrderRequest
    {
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
