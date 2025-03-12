using Google.Cloud.Firestore;

namespace Domain.Entities
{
    [FirestoreData]
    public class ShoppingCart : BaseEntity
    {
        [FirestoreProperty]
        public double TotalPrice { get; set; }
        [FirestoreProperty]
        public User? User { get; set; }
        [FirestoreProperty]
        public List<Product> Products { get; set; } = new();
    }
}
