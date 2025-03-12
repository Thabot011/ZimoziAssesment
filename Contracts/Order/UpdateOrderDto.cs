using Contracts.Product;
using Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Order
{
    public class UpdateOrderDto
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string ShippingAddress { get; set; }
        [Required]
        public PyamentMethod PaymentMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        public UserDto? User { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
