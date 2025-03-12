using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Order
{
    public class GetOrdersDto
    {
        [Required]
        public int PageSize { get; set; }
        [Required]

        public int PageNumber { get; set; }

        public string? UserId { get; set; }

        public string? FirstDocumentId { get; set; }
        public string? LastDocumentId { get; set; }
    }
}
