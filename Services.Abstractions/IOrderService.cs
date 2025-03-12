using Contracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task<string> AddOrder(CreateOrderDto order);
        Task<OrderDto> GetOrderById(string orderId);
        Task<AllOrdersDto> GetOrders(GetOrdersDto ordersDto);
        Task UpdateOrder(UpdateOrderDto order);
    }
}
