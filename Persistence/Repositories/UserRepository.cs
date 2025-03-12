using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly CollectionReference _collectionRef;


        public UserRepository()
        {
            _firestoreDb = FirestoreDb.Create("zimozi-ac7c3");
            _collectionRef = _firestoreDb.Collection(nameof(User));

        }
        public async Task<string> AddUser(User user)
        {
            var userDocRef = await _collectionRef.AddAsync(user);
            user.Id = userDocRef.Id;
            await UpdateUser(user);
            return userDocRef.Id;
        }

        public async Task<User> GetUser(string userId)
        {
            DocumentReference docRef = _collectionRef.Document(userId);
            DocumentSnapshot snapShot = await docRef.GetSnapshotAsync();
            User user = snapShot.ConvertTo<User>();
            user.Id = snapShot.Id;
            return user;
        }

        public async Task<User?> GetUserByUserIdentity(string userId)
        {
            Query query = _collectionRef.WhereEqualTo(nameof(User.UserId), userId);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
            {
                return null; // No user found with that name
            }

            // Assuming we expect only one document, get the first one
            DocumentSnapshot documentSnapshot = snapshot.Documents[0];

            // Convert the document to a User object (ensure you have a User class to match your Firestore structure)
            User user = documentSnapshot.ConvertTo<User>();
            user.Id = documentSnapshot.Id;
            return user;
        }

        public async Task UpdateUser(User user)
        {
            DocumentReference docRef = _collectionRef.Document(user.Id);
            await docRef.SetAsync(user);
        }
    }
}
