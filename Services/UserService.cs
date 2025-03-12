using Contracts.User;
using Domain.Reposiroty_Interfaces;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto?> GetUserById(string userId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                return null;
            }
            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                Role = (UserRole)user.Role,
                UserId = user.UserId,
                Orders = user.Orders.Select(x => new Contracts.Order.OrderDto
                {
                    Id = x.Id,
                    OrderStatus = (Contracts.Order.OrderStatus)x.OrderStatus,
                    PaymentMethod = (Contracts.Order.PyamentMethod)x.PaymentMethod,
                    ShippingAddress = x.ShippingAddress,

                }).ToList(),
                ShoppingCart = new Contracts.ShoppingCart.ShoppingCartDto
                {
                    Id = user.ShoppingCart?.Id,
                    TotalPrice = user.ShoppingCart?.TotalPrice
                }
            };
        }

        public async Task UpdateUser(UpdateUserDto user)
        {
            await _userRepository.UpdateUser(new Domain.Entities.User
            {
                FullName = user.FullName,
                Id = user.Id,
            });
        }
    }
}
