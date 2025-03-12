using Contracts.Product;
using Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ShoppingCart
{
    public class AddShoppingCartDto
    {
        public string? Id { get; set; }
        public double TotalPrice { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public required List<string> ProductIds { get; set; }
        public List<ProductDto> Products { get; set; }
        public bool IsLogin { get; set; }
    }
}
