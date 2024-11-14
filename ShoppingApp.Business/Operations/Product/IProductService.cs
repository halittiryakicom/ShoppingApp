using ShoppingApp.Business.Operations.Product.Dtos;
using ShoppingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Operations.Product
{
    public interface IProductService
    {
        Task<ServiceMessage> AddProduct(AddProductDto product);
        Task<ProductDto> GetProduct(int id);

        Task<List<ProductDto>> GetAllProducts();
        Task<ServiceMessage> AdjustQuantity(int id, int changeBy);
        Task<ServiceMessage> DeleteProduct(int id);
        Task<ServiceMessage> UpdateProduct(UpdateProductDto product);
    }
}
