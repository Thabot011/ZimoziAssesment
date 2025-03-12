using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Firebase.Auth;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly CollectionReference _cartCollectionRef;

        public ShoppingCartRepository()
        {
            _firestoreDb = FirestoreDb.Create("zimozi-ac7c3");
            _cartCollectionRef = _firestoreDb.Collection(nameof(ShoppingCart));

        }
        public async Task<string> AddCart(ShoppingCart shoppingCart)
        {
            var cartDocRef = await _cartCollectionRef.AddAsync(shoppingCart);
            shoppingCart.Id = cartDocRef.Id;
            await UpdateCart(shoppingCart);
            return cartDocRef.Id;
        }

        public async Task<ShoppingCart?> GetShoppingCartById(string cartId)
        {
            if (string.IsNullOrEmpty(cartId))
            {
                return null;
            }
            var cart = await _cartCollectionRef.Document(cartId).GetSnapshotAsync();

            if (cart == null)
            {
                return null; // No user found with that name
            }
            // Convert the document to a User object (ensure you have a User class to match your Firestore structure)
            ShoppingCart shoppingCart = cart.ConvertTo<ShoppingCart>();
            shoppingCart.Id = cart.Id;
            return shoppingCart;
        }

        public async Task UpdateCart(ShoppingCart shoppingCart)
        {
            DocumentReference docRef = _cartCollectionRef.Document(shoppingCart.Id);
            await docRef.SetAsync(shoppingCart);
        }
    }
}
