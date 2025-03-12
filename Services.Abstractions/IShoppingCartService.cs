using Contracts.ShoppingCart;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IShoppingCartService
    {
        Task<string> AddCart(AddShoppingCartDto shoppingCart);
        Task<ShoppingCartDto?> GetShoppingCart(string cartId);
        Task<ShoppingCartDto> RemoveFromCart(UpdateShoppingCartDto shoppingCart);
    }
}
