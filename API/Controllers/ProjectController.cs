using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        public ProjectController(ITraineeService traineeService,
            IProjectService projectService,
            IDirectionService directionService
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
        }

        [HttpPost("project")]
        public async Task<IActionResult> Create(ProjectDTO project)
        {
            try
            {
                await Task.Run(() => _projectService.Create(project));
                
                return Ok(project);
            }
            catch (Exception e)
            {
                return NotFound("Не уникальное имя проекта");
            }
        }
    }
}
