using ShoppingApp.Business.Operations.Order.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Operations.Product.Dtos
{
    public class OrderProductDto
    {
        public int OrderId { get; set; }
        public AddOrderDto Order { get; set; }

        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
