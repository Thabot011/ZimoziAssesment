using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Product
{
    public class GetProductsDto
    {
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }
        public string? FirstDocumentId { get; set; }
        public string? LastDocumentId { get; set; }
        public Category? Category { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }
    }
}
