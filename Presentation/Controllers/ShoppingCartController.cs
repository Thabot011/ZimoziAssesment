using Contracts.ShoppingCart;
using Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        [HttpPost]
        [Route("addProductToCart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddShoppingCartDto model)
        {
            var id = await _shoppingCartService.AddCart(model);
            return Ok(id);
        }

        [HttpPost]
        [Route("removeProductFromCart")]
        public async Task<IActionResult> RemoveproductFromCart([FromBody] UpdateShoppingCartDto model)
        {
            var cart = await _shoppingCartService.RemoveFromCart(model);
            return Ok(cart);
        }

        [HttpGet]
        [Route("getShoppingCart/{cartId}")]
        public async Task<IActionResult> GetShoppingCart([FromRoute] string cartId)
        {
            var cart = await _shoppingCartService.GetShoppingCart(cartId);
            return Ok(cart);
        }
    }
}
