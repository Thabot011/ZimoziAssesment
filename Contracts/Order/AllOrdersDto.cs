namespace Contracts.Order
{
    public class AllOrdersDto
    {
        public int TotalCount { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
