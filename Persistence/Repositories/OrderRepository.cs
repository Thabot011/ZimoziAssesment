using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly CollectionReference _collectionRef;


        public OrderRepository()
        {
            _firestoreDb = FirestoreDb.Create("zimozi-ac7c3");
            _collectionRef = _firestoreDb.Collection(nameof(Order));

        }
        public async Task<string> AddOrder(Order order)
        {
            var orderDocRef = await _collectionRef.AddAsync(order);
            order.Id = orderDocRef.Id;
            await UpdateOrder(order);
            return orderDocRef.Id;
        }

        public async Task DeleteOrder(string OrderId)
        {
            DocumentReference docRef = _collectionRef.Document(OrderId);
            await docRef.DeleteAsync();
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            DocumentReference docRef = _collectionRef.Document(orderId);
            DocumentSnapshot snapShot = await docRef.GetSnapshotAsync();
            Order order = snapShot.ConvertTo<Order>();
            order.Id = snapShot.Id;
            return order;
        }

        public async Task<(IEnumerable<Order>, int)> GetOrders(int pageSize, int pageNumber, string userId, List<string> orderIds, string? firstDocumentId, string? lastDocumentId)
        {
            Query query = null;
            int limit = pageSize;
            query = _collectionRef.OrderByDescending(nameof(Order.CreatedDate));
            if (!string.IsNullOrEmpty(userId) && orderIds.Any())
            {
                query = query.WhereIn(nameof(Order.Id), orderIds);
            }
            else if (!string.IsNullOrEmpty(userId) && !orderIds.Any())
            {
                limit = 0;
            }

            var totalCount = (await query.GetSnapshotAsync()).Count;

            query = query.Offset(pageNumber * pageSize);

            var pagedQuery = query.Limit(limit);

            // Execute the query
            QuerySnapshot snapshot = await pagedQuery.GetSnapshotAsync();

            List<Order> documents = new List<Order>();

            // List the documents returned from Firestore
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Order document = documentSnapshot.ConvertTo<Order>();
                document.Id = documentSnapshot.Id;
                documents.Add(document);
            }
            return (documents, totalCount);
        }

        public async Task UpdateOrder(Order order)
        {
            DocumentReference docRef = _collectionRef.Document(order.Id);
            await docRef.SetAsync(order);
        }
    }
}
