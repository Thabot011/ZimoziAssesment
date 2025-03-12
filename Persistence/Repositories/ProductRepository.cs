using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly FirestoreDb _firestoreDb;
        private readonly CollectionReference _collectionRef;


        public ProductRepository()
        {
            _firestoreDb = FirestoreDb.Create("zimozi-ac7c3");
            _collectionRef = _firestoreDb.Collection(nameof(Product));
        }
        public async Task<string> AddProduct(Product product)
        {
            var productDocRef = await _collectionRef.AddAsync(product);
            product.Id = productDocRef.Id;
            await UpdateProduct(product);
            return productDocRef.Id;
        }

        public async Task DeleteProduct(string productId)
        {
            DocumentReference docRef = _collectionRef.Document(productId);
            await docRef.DeleteAsync();
        }

        public async Task<Product> GetProductById(string productId)
        {
            DocumentReference docRef = _collectionRef.Document(productId);
            DocumentSnapshot snapShot = await docRef.GetSnapshotAsync();
            Product product = snapShot.ConvertTo<Product>();
            product.Id = snapShot.Id;
            return product;
        }

        public async Task<(IEnumerable<Product>, int)> GetProducts(int pageSize, int pageNumber, string? firstDocumentId, string? lastDocumentId, Category? categoryFilter, double? priceFrom, double? priceTo)
        {
            var query = _collectionRef.OrderByDescending(nameof(Product.CreatedDate));
            if (categoryFilter.HasValue)
            {
                query = query.WhereEqualTo(nameof(Product.Category), categoryFilter.Value);
            }

            if (priceFrom.HasValue)
            {
                query = query.WhereGreaterThanOrEqualTo(nameof(Product.Price), priceFrom);
            }

            if (priceTo.HasValue)
            {
                query = query.WhereLessThanOrEqualTo(nameof(Product.Price), priceTo);
            }

            // Execute the query
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            var totalCount = snapshot.Count;

            query = query.Offset(pageNumber * pageSize);

            var pagedQuery = query.Limit(pageSize);

            var pagedSnashot = await pagedQuery.GetSnapshotAsync();


            List<Product> documents = new List<Product>();

            // List the documents returned from Firestore
            foreach (DocumentSnapshot documentSnapshot in pagedSnashot.Documents)
            {
                Product document = documentSnapshot.ConvertTo<Product>();
                document.Id = documentSnapshot.Id;
                documents.Add(document);
            }
            return (documents, totalCount);
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(List<string> productIds)
        {
            Query query = null;
            if (productIds.Any())
            {
                query = _collectionRef.WhereIn(FieldPath.DocumentId, productIds);
            }
            else
            {
                query= _collectionRef.Limit(0);
            }
            var querySnapshot = await query.GetSnapshotAsync();
            List<Product> products = new List<Product>();
            foreach (var document in querySnapshot.Documents)
            {
                var product = document.ConvertTo<Product>();
                product.Id = document.Id;
                products.Add(product);
            }

            return products;
        }

        public async Task UpdateProduct(Product product)
        {
            DocumentReference docRef = _collectionRef.Document(product.Id);
            await docRef.SetAsync(product);
        }

        public async Task BulkUpdateProduct(List<Product> products)
        {
            var batch = _firestoreDb.StartBatch();
            foreach (var product in products)
            {
                DocumentReference docRef = _collectionRef.Document(product.Id);
                await docRef.SetAsync(product);
            }
        }

    }
}
