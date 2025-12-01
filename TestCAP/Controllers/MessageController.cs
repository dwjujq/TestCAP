using Microsoft.AspNetCore.Mvc;

namespace TestCAP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        public MessageController() { }

        [NonAction]
        public async Task SendMessage()
        {

        }
    }
}
