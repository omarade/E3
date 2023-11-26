using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInboundConnenction()
        {
            Console.WriteLine("----> Inbound post at Commands Service");
            return Ok("Inbound test ok");
        }
    }
}