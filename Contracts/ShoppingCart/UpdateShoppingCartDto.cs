using Contracts.Product;
using Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ShoppingCart
{
    public class UpdateShoppingCartDto
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public required List<ProductDto> Products { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
