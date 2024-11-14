using ShoppingApp.Business.Operations.Order.Dtos;
using ShoppingApp.Business.Operations.Product.Dtos;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Repositories;
using ShoppingApp.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<OrderEntity> _orderRepository;

        public OrderManager (IUnitOfWork unitOfWork, IRepository<ProductEntity> productRepository, IRepository<OrderEntity> orderRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto order)
        {


            var orderEntity = new OrderEntity
            {
                OrderDate = DateTime.Now,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId,
            };

            _orderRepository.Add(orderEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Sipariş oluşturulurken bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş kaydı tamamlandı."
            };
        }
    }
}
