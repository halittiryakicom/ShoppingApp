using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Operations.Order;
using ShoppingApp.Business.Operations.Product.Dtos;
using ShoppingApp.Business.Operations.Product;
using ShoppingApp.WebApi.Models;
using ShoppingApp.Business.Operations.Order.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            var orderDto = new AddOrderDto
            {
                OrderDate = DateTime.Now,
                TotalAmount = request.TotalAmount,
                CustomerId = request.CustomerId,
            };
            var result = await _orderService.AddOrder(orderDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
