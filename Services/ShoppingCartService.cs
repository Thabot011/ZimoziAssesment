using Contracts.Product;
using Contracts.ShoppingCart;
using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Microsoft.VisualBasic;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<string> AddCart(AddShoppingCartDto shoppingCart)
        {
            var products = await _productRepository.GetProductsByIds(shoppingCart.ProductIds);
            if (!shoppingCart.IsLogin)
            {
                products.ToList().ForEach((p) =>
                {
                    p.StockAvailability = p.StockAvailability == 0 ? 0 : p.StockAvailability - 1;
                });
                await _productRepository.BulkUpdateProduct(products.ToList());
                shoppingCart.Products.ForEach((p) =>
                {
                    p.StockAvailability = p.StockAvailability == 0 ? 0 : p.StockAvailability - 1;
                });
            }


            var cart = await GetShoppingCart(shoppingCart.Id);
            if (cart is null)
            {
                var user = await _userRepository.GetUser(shoppingCart.UserId);
                var cartId = await _shoppingCartRepository.AddCart(new Domain.Entities.ShoppingCart
                {
                    Products = shoppingCart.Products.Select(x => new Product()
                    {
                        Id = x.Id,
                        Category = (Domain.Entities.Category)x.Category,
                        ImagePath = x.ImagePath,
                        Name = x.Name,
                        Price = x.Price,
                        StockAvailability = x.StockAvailability,
                    }).ToList(),
                    TotalPrice = shoppingCart.Products.Sum(p => p.Price),
                    User = user
                });

                var userCart = await _shoppingCartRepository.GetShoppingCartById(cartId);

                user.ShoppingCart = userCart;

                await _userRepository.UpdateUser(user);
                return cartId;
            }
            else
            {
                var user = await _userRepository.GetUser(shoppingCart.UserId);
                cart.Products.Where(x => shoppingCart.Products.Select(y => y.Id).Contains(x.Id)).ToList().ForEach((p) =>
                {
                    p.StockAvailability = p.StockAvailability == 0 ? 0 : p.StockAvailability - 1;
                });

                await UpdateCart(new UpdateShoppingCartDto
                {
                    Id = cart.Id,
                    Products = shoppingCart.Products.Select(x => new Contracts.Product.ProductDto
                    {
                        Id = x.Id,
                        Category = (Contracts.Product.Category)x.Category,
                        ImagePath = x.ImagePath,
                        Name = x.Name,
                        Price = x.Price,
                        StockAvailability = x.StockAvailability
                    }).ToList().Concat(cart.Products).ToList(),
                    TotalPrice = products.Select(x => x.Price).Concat(cart.Products.Select(x => x.Price)).Sum(p => p),
                });

                var userCart = await _shoppingCartRepository.GetShoppingCartById(shoppingCart.Id);

                user.ShoppingCart = userCart;

                await _userRepository.UpdateUser(user);

                return shoppingCart.Id;
            }
        }

        public async Task<ShoppingCartDto?> GetShoppingCart(string cartId)
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartById(cartId);
            if (shoppingCart is null)
            {
                return null;
            }
            return new ShoppingCartDto()
            {
                Id = shoppingCart.Id,
                ProductIds = shoppingCart.Products.Select(x => x.Id).ToList(),
                Products = shoppingCart.Products.Select(x => new Contracts.Product.ProductDto
                {
                    ImagePath = x.ImagePath,
                    Name = x.Name,
                    Category = (Contracts.Product.Category)x.Category,
                    Id = x.Id,
                    Price = x.Price,
                    StockAvailability = x.StockAvailability
                }).ToList(),
                TotalPrice = shoppingCart.TotalPrice
            };
        }

        public async Task<ShoppingCartDto> RemoveFromCart(UpdateShoppingCartDto shoppingCart)
        {
            var product = await _productRepository.GetProductById(shoppingCart.ProductId);

            product.StockAvailability = product.StockAvailability + 1;

            await _productRepository.UpdateProduct(product);

            shoppingCart.Products.Where(x => x.Id == product.Id).ToList().ForEach((p) =>
            {
                p.StockAvailability = p.StockAvailability + 1;

            });


            await UpdateCart(new UpdateShoppingCartDto
            {
                Id = shoppingCart.Id,
                Products = shoppingCart.Products,
                TotalPrice = shoppingCart.Products.Sum(p => p.Price),
            });

            var user = await _userRepository.GetUser(shoppingCart.UserId);

            var cart = await _shoppingCartRepository.GetShoppingCartById(shoppingCart.Id);

            user.ShoppingCart = cart;

            await _userRepository.UpdateUser(user);

            return new ShoppingCartDto
            {
                Id = user.ShoppingCart.Id,
                TotalPrice = user.ShoppingCart.TotalPrice,
                ProductIds = user.ShoppingCart.Products.Select(x => x.Id).ToList(),
                Products = user.ShoppingCart.Products.Select(x => new ProductDto
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    Category = (Contracts.Product.Category)x.Category,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailability = x.StockAvailability,
                }).ToList(),
            };

        }

        public async Task UpdateCart(UpdateShoppingCartDto shoppingCart)
        {
            await _shoppingCartRepository.UpdateCart(new Domain.Entities.ShoppingCart()
            {
                Id = shoppingCart.Id,
                TotalPrice = shoppingCart.TotalPrice,
                Products = shoppingCart.Products.Select(x => new Product
                {
                    ImagePath = x.ImagePath,
                    Name = x.Name,
                    Category = (Domain.Entities.Category)x.Category,
                    Id = x.Id,
                    Price = x.Price,
                    StockAvailability = x.StockAvailability
                }).ToList()
            });
        }
    }
}
