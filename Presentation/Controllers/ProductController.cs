using Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("addProduct")]
        [Authorize(Roles = "0")] //Admin role
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto model)
        {
            var id = await _productService.AddProduct(model);
            return Ok(id);
        }

        [HttpPut]
        [Route("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto model)
        {
            await _productService.UpdateProduct(model);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteProduct/{ProductId}")]
        [Authorize(Roles = "0")] //Admin role
        public async Task<IActionResult> DeleteProduct([FromRoute] string productId)
        {
            await _productService.DeleteProduct(productId);
            return Ok();
        }

        [HttpPost]
        [Route("getAllProducts")]
        public async Task<IActionResult> GetAllproducts([FromBody] GetProductsDto model)
        {
            var products = await _productService.GetProducts(model);
            return Ok(products);
        }

        [HttpGet]
        [Route("getProductById/{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute] string productId)
        {
            var products = await _productService.GetProductById(productId);
            return Ok(products);
        }
    }
}
