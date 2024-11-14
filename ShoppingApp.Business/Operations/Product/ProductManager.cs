using Microsoft.EntityFrameworkCore;
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

namespace ShoppingApp.Business.Operations.Product
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> productRepository, IRepository<OrderProductEntity> orderProductRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _orderProductRepository = orderProductRepository;
        }
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
           var hasProduct = _productRepository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();
            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu üründen elimizde bulunuyor"
                };
            }

            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
            };

            _productRepository.Add(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ürün kayıdı sırasında bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün kayıdı tamamlandı"
            };
        }

        public async Task<ServiceMessage> AdjustQuantity(int id, int changeBy)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eşleşen ürün bulunamadı"
                };
            }

            product.StockQuantity = changeBy;
            _productRepository.Update(product);


            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Stok miktarı değiştirilirken bir hata oldu");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Stok başarı ile güncellendi"
            };
        }

        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sİlinmek istenenen ürün bulunamadı"
                };
            }
            _productRepository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Silme işlemi sırasında bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var product = await _productRepository.GetAll()
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    ProductName = x.ProductName,
                    Orders = x.Orders.Select(
                        f => new OrderProductDto
                        {
                            OrderId = f.OrderId,
                            ProductId = f.ProductId,
                            Quantity = f.Quantity,
                        }).ToList()
                }).ToListAsync();
            return product;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetAll(x => x.Id == id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    ProductName = x.ProductName,
                    Orders = x.Orders.Select(
                        f => new OrderProductDto
                        {
                            OrderId = f.Id,
                            ProductId = f.ProductId,
                            Quantity = f.Quantity,
                        }).ToList()
                }).FirstOrDefaultAsync();
            return product;
        }

        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _productRepository.GetById(product.Id);
            if (productEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı"
                };
            }

            await _unitOfWork.BeginTransaction();
            productEntity.ProductName = product.ProductName;
            productEntity.StockQuantity = product.StockQuantity;
            productEntity.Price = product.Price;
            _productRepository.Update(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Ürün bilgileri güncellenirken bir hata ile karşılaşıldı");
            }

            var orderProducts = _orderProductRepository.GetAll(x => x.ProductId == product.Id).ToList();
            foreach (var orderProduct in orderProducts)
            {
                _orderProductRepository.Delete(orderProduct, false);
            }


            foreach(var orderProductId in product.OrderProductsId)
            {
                var orderProduct = new OrderProductEntity
                {
                    ProductId = orderProductId,
                    OrderId = orderProductId,
                };
                _orderProductRepository.Add(orderProduct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Ürün bilgileri güncellenirken bir hata ile karşılaşıldı işlemler geri alınıyor");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
            };
        }
    }
}
