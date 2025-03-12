using Google.Cloud.Firestore;

namespace Domain.Entities
{
    [FirestoreData]
    public class User : BaseEntity
    {
        [FirestoreProperty]
        public required string FullName { get; set; }
        [FirestoreProperty]
        public string? Email { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public UserRole Role { get; set; }
        [FirestoreProperty]
        public List<Order> Orders { get; set; } = new();
        [FirestoreProperty]
        public ShoppingCart? ShoppingCart { get; set; }
    }
    public enum UserRole
    {
        Admin,
        NormalUser
    }
}
