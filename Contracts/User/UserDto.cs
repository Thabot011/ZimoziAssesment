using Contracts.Order;
using Contracts.ShoppingCart;

namespace Contracts.User
{
    public class UserDto
    {
        public string? Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string UserId { get; set; }
        public UserRole Role { get; set; }
        public string? Token { get; set; }
        public ShoppingCartDto? ShoppingCart { get; set; }
        public List<OrderDto>? Orders { get; set; }
    }
}
