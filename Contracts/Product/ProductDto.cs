using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Product
{
    public class ProductDto
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public required string ImagePath { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public int StockAvailability { get; set; }
    }
    public enum Category
    {
        Food,
        Drink,
        Clothes
    }
}
