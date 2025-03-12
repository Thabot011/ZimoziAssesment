using Domain.Entities;

namespace Domain.Reposiroty_Interfaces
{
    public interface IProductRepository
    {
        Task<string> AddProduct(Product product);
        Task<Product> GetProductById(string productId);
        Task<(IEnumerable<Product>, int)> GetProducts(int pageSize, int pageNumber, string? firstDocumentId, string? lastDocumentId, Category? categoryFilter, double? priceFrom, double? priceTo);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string productId);
        Task<IEnumerable<Product>> GetProductsByIds(List<string> productIds);
        Task BulkUpdateProduct(List<Product> products);
    }
}
