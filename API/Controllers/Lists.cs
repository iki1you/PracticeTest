using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Lists : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        public Lists(ITraineeService traineeService,
            IProjectService projectService,
            IDirectionService directionService
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
        }

        [HttpGet("lists")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Projects = await Task.Run(() => _projectService.GetAll());
            ViewBag.Directions = await Task.Run(() => _directionService.GetAll());
            return View();
        }
    }
}
