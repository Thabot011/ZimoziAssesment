using Contracts.Order;
using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<string> AddOrder(CreateOrderDto order)
        {
            var products = await _productRepository.GetProductsByIds(order.ProductIds);
            var user = await _userRepository.GetUser(order.User.Id);
            var orderEntity = new Domain.Entities.Order
            {
                OrderStatus = (Domain.Entities.OrderStatus)order.OrderStatus,
                PaymentMethod = (Domain.Entities.PyamentMethod)order.PaymentMethod,
                Products = products.ToList(),
                ShippingAddress = order.ShippingAddress,
                User = user
            };
            var orderId = await _orderRepository.AddOrder(orderEntity);
            orderEntity.Id = orderId;
            orderEntity.User = null;
            user.Orders = user.Orders.Append(orderEntity).ToList();

            user.ShoppingCart = new ShoppingCart();
            await _userRepository.UpdateUser(user);
            return orderId;
        }

        public async Task<OrderDto> GetOrderById(string orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            return new OrderDto
            {
                Id = order.Id,
                OrderStatus = (Contracts.Order.OrderStatus)order.OrderStatus,
                PaymentMethod = (Contracts.Order.PyamentMethod)order.PaymentMethod,
                Products = order.Products.Select(x => new Contracts.Product.ProductDto
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    Category = (Contracts.Product.Category)x.Category,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailability = x.StockAvailability
                }).ToList(),
                ShippingAddress = order.ShippingAddress
            };
        }

        public async Task<AllOrdersDto> GetOrders(GetOrdersDto ordersDto)
        {
            User user = null;

            if (!string.IsNullOrEmpty(ordersDto.UserId))
            {
                user = await _userRepository.GetUser(ordersDto.UserId);
            }

            var (orders, totalCount) = await _orderRepository.GetOrders(ordersDto.PageSize, ordersDto.PageNumber, ordersDto.UserId, user?.Orders?.Select(x => x.Id).ToList(), ordersDto.FirstDocumentId, ordersDto.LastDocumentId);
            var orderDtos = orders.Select(x => new OrderDto
            {
                Id = x.Id,
                OrderStatus = (Contracts.Order.OrderStatus)x.OrderStatus,
                PaymentMethod = (Contracts.Order.PyamentMethod)x.PaymentMethod,
                Products = x.Products.Select(y => new Contracts.Product.ProductDto
                {
                    Id = y.Id,
                    Category = (Contracts.Product.Category)y.Category,
                    ImagePath = y.ImagePath,
                    Name = y.Name,
                    Price = y.Price,
                    StockAvailability = y.StockAvailability
                }).ToList(),
                ShippingAddress = x.ShippingAddress,
                User = x.User != null ? new Contracts.User.UserDto
                {
                    Email = x.User?.Email,
                    FullName = x.User?.FullName,
                    Id = x.User?.Id,
                    UserId = x.User?.UserId,
                    Role = (Contracts.User.UserRole)x.User?.Role,
                } : null
            }).ToList();

            var allOrdersDto = new AllOrdersDto
            {
                Orders = orderDtos,
                TotalCount = totalCount
            };
            return allOrdersDto;
        }

        public async Task UpdateOrder(UpdateOrderDto order)
        {
            await _orderRepository.UpdateOrder(new Domain.Entities.Order
            {
                Id = order.Id,
                OrderStatus = (Domain.Entities.OrderStatus)order.OrderStatus,
                PaymentMethod = (Domain.Entities.PyamentMethod)order.PaymentMethod,
                Products = order.Products.Select(x => new Product
                {
                    Id = x.Id,
                    Category = (Category)x.Category,
                    ImagePath = x.ImagePath,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailability = order.OrderStatus == Contracts.Order.OrderStatus.Delivered ? x.StockAvailability - 1 : x.StockAvailability,
                }).ToList(),
                User = new User
                {
                    FullName = order.User.FullName,
                    Id = order.User.Id,
                    Email = order.User.Email,
                    Role = (UserRole)order.User.Role,
                    UserId = order.User.UserId
                },
                ShippingAddress = order.ShippingAddress
            });
            if (order.OrderStatus == Contracts.Order.OrderStatus.Delivered)
            {

                var products = order.Products.GroupBy(x => x.Id).Select(x => new Product
                {
                    Id = x.FirstOrDefault().Id,
                    ImagePath = x.FirstOrDefault().ImagePath,
                    Name = x.FirstOrDefault().Name,
                    Price = x.FirstOrDefault().Price,
                    Category = (Category)x.FirstOrDefault().Category,
                    StockAvailability = x.FirstOrDefault().StockAvailability,
                });

                await _productRepository.BulkUpdateProduct(products.ToList());

                var user = await _userRepository.GetUser(order.User.Id);
                user.ShoppingCart = new ShoppingCart();
                await _userRepository.UpdateUser(user);
            }
        }
    }
}
