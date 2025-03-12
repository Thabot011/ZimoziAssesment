using Contracts.Product;
using Contracts.User;

namespace Contracts.Order
{
    public class OrderDto
    {
        public string? Id { get; set; }
        public required string ShippingAddress { get; set; }
        public PyamentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public UserDto? User { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
