using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Order.API.Dtos;
using Order.API.Entities;
using Order.API.Message;

namespace Order.API.Services
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _dbContext;
        private readonly ICapPublisher _capPublisher;

        public OrderService(AppDbContext dbContext,ICapPublisher capPublisher)
        {
            _dbContext = dbContext;
            _capPublisher = capPublisher;
        }

        public async Task<OrderDto> CreatOrder(OrderDto orderDto)
        {
            using (var trans = _dbContext.Database.BeginTransaction(_capPublisher, autoCommit: true))
            {
                var order = new Entities.Order
                {
                    Id = Guid.NewGuid(),
                    ProductId = orderDto.ProductId,
                    Quantity = orderDto.Quantity,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Order.Add(order);
                var flag = await _dbContext.SaveChangesAsync();

                throw new Exception("ewrrrrrr");

                if (flag == 1)
                {
                    orderDto.Id = order.Id;

                    var messageData = new MessageData<OrderDto>(orderDto);
                    await _capPublisher.PublishAsync("order.created", messageData);

                    return orderDto;
                }
                return null;
            }
        }
    }
}
