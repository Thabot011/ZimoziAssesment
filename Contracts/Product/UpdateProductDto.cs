using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Product
{
    public class UpdateProductDto
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int StockAvailability { get; set; }
    }
}
