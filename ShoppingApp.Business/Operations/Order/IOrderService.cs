using ShoppingApp.Business.Operations.Order.Dtos;
using ShoppingApp.Business.Operations.Product.Dtos;
using ShoppingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrder(AddOrderDto order);
    }
}
