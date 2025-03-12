using Contracts.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("addOrder")]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDto model)
        {
           string id = await _orderService.AddOrder(model);
            return Ok(id);
        }

        [HttpPut]
        [Route("updateOrder")]
        [Authorize(Roles = "0")] //Admin role
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto model)
        {
            await _orderService.UpdateOrder(model);
            return Ok();
        }

        [HttpPost]
        [Route("getOrders")]
        public async Task<IActionResult> GetOrders([FromBody] GetOrdersDto model)
        {
            var orders = await _orderService.GetOrders(model);
            return Ok(orders);
        }
    }
}
