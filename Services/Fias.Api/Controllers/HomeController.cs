using Microsoft.AspNetCore.Mvc;

namespace Fias.Api.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("pong");
        }
    }
}
