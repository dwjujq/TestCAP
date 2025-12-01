using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;
using Order.API.Message;
using Order.API.Services;

namespace Order.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]OrderDto orderDto)
        {
            var order =await _orderService.CreatOrder(orderDto);
            return Ok();
        }
    }
}
