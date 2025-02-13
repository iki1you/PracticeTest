using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
