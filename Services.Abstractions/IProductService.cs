using Contracts.Product;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task<string> AddProduct(AddProductDto product);
        Task<ProductDto> GetProductById(string productId);
        Task<AllProductsDto> GetProducts(GetProductsDto productsDto);
        Task UpdateProduct(UpdateProductDto product);
        Task DeleteProduct(string productId);
    }
}
