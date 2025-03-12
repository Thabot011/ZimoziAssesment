using Contracts.Order;
using Contracts.ShoppingCart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.User
{
    public class UpdateUserDto
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string FullName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public UserRole Role { get; set; }
        public string? Token { get; set; }
        public ShoppingCartDto? ShoppingCart { get; set; }
        public List<OrderDto>? Orders { get; set; }
    }
}
