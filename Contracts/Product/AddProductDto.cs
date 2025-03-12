using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Product
{
    public class AddProductDto
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int StockAvailability { get; set; }
    }
}
