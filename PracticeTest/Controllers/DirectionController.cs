using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class DirectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
