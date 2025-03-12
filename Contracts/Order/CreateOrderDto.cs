using Contracts.Product;
using Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Order
{
    public class CreateOrderDto
    {
        [Required]
        [MaxLength(100)]
        public required string ShippingAddress { get; set; }
        [Required]
        public PyamentMethod PaymentMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        public UserDto User { get; set; }
        [Required]
        public required List<string> ProductIds { get; set; }
    }
    public enum PyamentMethod
    {
        Cash,
        Visa
    }

    public enum OrderStatus
    {
        Pending, Shipped, Delivered
    }
}
