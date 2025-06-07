using Microsoft.AspNetCore.Mvc;

namespace JerEntryWebApp.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("Ok");
        }
    }
}
