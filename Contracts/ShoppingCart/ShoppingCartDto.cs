using Contracts.Product;
using Contracts.User;

namespace Contracts.ShoppingCart
{
    public class ShoppingCartDto
    {
        public string? Id { get; set; }
        public double? TotalPrice { get; set; }
        public UserDto? User { get; set; }
        public List<string>? ProductIds { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
