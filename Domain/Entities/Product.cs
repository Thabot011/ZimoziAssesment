using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [FirestoreData]
    public class Product : BaseEntity
    {
        [FirestoreProperty]
        public required string Name { get; set; }
        [FirestoreProperty]
        public required string ImagePath { get; set; }
        [FirestoreProperty]
        public double Price { get; set; }
        [FirestoreProperty]
        public Category Category { get; set; }
        [FirestoreProperty]
        public int StockAvailability { get; set; }
    }
    public enum Category
    {
        Food,
        Drink,
        Tools
    }
}
