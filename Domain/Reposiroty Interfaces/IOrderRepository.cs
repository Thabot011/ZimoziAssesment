using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reposiroty_Interfaces
{
    public interface IOrderRepository
    {
        Task<string> AddOrder(Order order);
        Task<Order> GetOrderById(string orderId);
        Task<(IEnumerable<Order>, int)> GetOrders(int pageSize, int pageNumber, string userId, List<string> orderIds, string? firstDocumentId, string? lastDocumentId);
        Task UpdateOrder(Order order);
        Task DeleteOrder(string OrderId);
    }
}
