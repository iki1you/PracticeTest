using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraineeController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        private readonly ILogger<TraineeController> _logger;
        public TraineeController(ITraineeService traineeService, 
            IProjectService projectService, 
            IDirectionService directionService,
            ILogger<TraineeController> logger
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
            _logger = logger;
        }


        [HttpPost("create")]
        public async Task<ActionResult> Create(TraineeDTO traineeDto)
        {
            string message = "Форма успешно отправлена";
            try
            {
                await Task.Run(() => _traineeService.Create(traineeDto));
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return Ok(message);
        }
    }
}
