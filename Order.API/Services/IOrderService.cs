using Order.API.Dtos;

namespace Order.API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreatOrder(OrderDto orderDto);
    }
}
