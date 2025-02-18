using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectionController : Controller
    {
        [HttpGet("project")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
