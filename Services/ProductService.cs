using Contracts.Product;
using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Microsoft.AspNetCore.Http;
using Services.Abstractions;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> AddProduct(AddProductDto product)
        {
            var filePath = await UploadImageAsync(product.Image);
            return await _productRepository.AddProduct(new Product
            {
                Category = (Domain.Entities.Category)product.Category,
                ImagePath = filePath,
                Name = product.Name,
                Price = product.Price,
                StockAvailability = product.StockAvailability
            });
        }

        public async Task DeleteProduct(string productId)
        {
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<ProductDto> GetProductById(string productId)
        {
            var product = await _productRepository.GetProductById(productId);
            return new ProductDto
            {
                Id = product.Id,
                ImagePath = product.ImagePath,
                Name = product.Name,
                Category = (Contracts.Product.Category)product.Category,
                Price = product.Price,
                StockAvailability = product.StockAvailability
            };
        }

        public async Task<AllProductsDto> GetProducts(GetProductsDto productsDto)
        {
            var (products, totalCount) = await _productRepository.GetProducts(productsDto.PageSize, productsDto.PageNumber, productsDto.FirstDocumentId, productsDto.LastDocumentId, (Domain.Entities.Category?)productsDto.Category, productsDto.PriceFrom, productsDto.PriceTo);

            var productDtos = products.Select(x => new ProductDto { ImagePath = x.ImagePath, Id = x.Id, Category = (Contracts.Product.Category)x.Category, Name = x.Name, Price = x.Price, StockAvailability = x.StockAvailability });

            var getAllProducts = new AllProductsDto() { Products = productDtos.ToList(), TotalCount = totalCount };
            return getAllProducts;
        }

        public async Task UpdateProduct(UpdateProductDto product)
        {

            var filePath = await UploadImageAsync(product.Image);
            await _productRepository.UpdateProduct(new Domain.Entities.Product
            {
                Id = product.Id,
                Category = (Domain.Entities.Category)product.Category,
                ImagePath = string.IsNullOrEmpty(filePath) ? product.ImagePath : filePath,
                Name = product.Name,
                Price = product.Price,
                StockAvailability = product.StockAvailability
            });
        }

        private async Task<string> UploadImageAsync(IFormFile image)
        {
            string filePath = string.Empty;
            string relativeFilePath = string.Empty;
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (image != null && image.Length > 0)
            {

                filePath = Path.Combine(directoryPath, image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                relativeFilePath = Path.Combine("images", image.FileName).Replace("\\", "/");
            }

            return relativeFilePath;
        }
    }
}
