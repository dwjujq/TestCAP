using DotNetCore.CAP;
using Message.API.Dtos;
using Message.API.Message;
using Message.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Message.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]InventoryDto inventoryDto)
        {
            var result=await _inventoryService.Create(inventoryDto);
            return Ok(result);
        }

        [CapSubscribe("order.created")]
        [NonAction]
        public async Task OrderCreated(MessageData<OrderDto> messageData)
        {
            //if (_messageTracker.HasProcessed(messageData.Id))
                //return;

            _logger.LogInformation($"消息{messageData.Id}的消息体：" + JsonConvert.SerializeObject(messageData.MessageBody));
            await _inventoryService.DeductInventory(messageData.MessageBody);

            //_messageTracker.MarkAsProcessed(messageData.Id);
        }

    }
}
