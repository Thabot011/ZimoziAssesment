using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [FirestoreData]
    public class Order : BaseEntity
    {
        [FirestoreProperty]
        public required string ShippingAddress { get; set; }
        [FirestoreProperty]
        public PyamentMethod PaymentMethod { get; set; }
        [FirestoreProperty]
        public OrderStatus OrderStatus { get; set; }
        [FirestoreProperty]
        public User? User { get; set; }
        [FirestoreProperty]
        public List<Product> Products { get; set; }
    }
    public enum PyamentMethod
    {
        Cash,
        Visa
    }

    public enum OrderStatus
    {
        Pending, Shipped, Delivered
    }
}
