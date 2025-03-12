using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reposiroty_Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<string> AddCart(ShoppingCart shoppingCart);
        Task<ShoppingCart?> GetShoppingCartById(string cartId);
        Task UpdateCart(ShoppingCart shoppingCart);
    }
}
