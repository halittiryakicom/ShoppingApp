using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Operations.Product;
using ShoppingApp.Business.Operations.Product.Dtos;
using ShoppingApp.WebApi.Filter;
using ShoppingApp.WebApi.Models;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var productDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };
            var result = await _productService.AddProduct(productDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPatch("{id}/quantity")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustProductQuantity(int id, int changeBy)
        {
            var result = await _productService.AdjustQuantity(id, changeBy);

            if (!result.IsSucceed)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPut("{id}")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var updateProductDto = new UpdateProductDto
            {
                Id = id,
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                OrderProductsId = request.OrderProductsId,
            };
            var result = await _productService.UpdateProduct(updateProductDto);

            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok();
            }

        }
    }
}
