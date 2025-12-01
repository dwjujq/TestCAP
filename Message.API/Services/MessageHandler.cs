using DotNetCore.CAP;
using Message.API.Dtos;

namespace Message.API.Services
{
    public class MessageHandler: ICapSubscribe
    {
        [CapSubscribe("orderCreate")]
        public async Task Handle(OrderDto eventData)
        {

        }
    }
}
