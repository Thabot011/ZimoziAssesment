using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [FirestoreData]
    public class BaseEntity
    {
        [FirestoreProperty]
        public string? Id { get; set; }
        [FirestoreProperty]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
