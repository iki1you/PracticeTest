using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PracticeTest.Controllers;
using WebApi.Models;
using WebApi.SignalRHubs;

namespace WebApi.Controllers
{
    public class TraineeController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        public TraineeController(ITraineeService traineeService, 
            IProjectService projectService, 
            IDirectionService directionService,
            ILogger<HomeController> logger,
            IHubContext<NotificationHub> hubContext
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("/trainee-create")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Projects = new SelectList(await _projectService.GetAll(), "Id", "Name");
            ViewBag.Directions = new SelectList(await _directionService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateViewModel model)
        {
            if (model.Project != null)
            {
                try
                {
                    await _projectService.Create(model.Project);
                    TempData["Message"] = "Проект создан";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }     
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirection(CreateViewModel model)
        {
            if (model.Direction != null)
            {
                try
                {
                    await _directionService.Create(model.Direction);
                    TempData["Message"] = "Направление создано";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("/trainee-create")]
        public async Task<ActionResult> CreateAsync(CreateViewModel model)
        {
            var traineeDto = model.Trainee;
            traineeDto.Project = model.Project;
            traineeDto.Direction = model.Direction;
            TraineeDTO trainee;
            string message = "Форма успешно отправлена";
            try
            {
                await _traineeService.Create(traineeDto);
                trainee = await _traineeService.Retrieve(traineeDto.Email);
                traineeDto.Project = trainee.Project;
                traineeDto.Direction = trainee.Direction;
                traineeDto.Id = trainee.Id;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            TempData["Message"] = message;
            var notification = new Dictionary<string, string>
            {
                { "id", traineeDto.Id.ToString() },
                { "name", traineeDto.Name },
                { "surname", traineeDto.Surname },
                { "gender", traineeDto.Gender.ToString() },
                { "email", traineeDto.Email },
                { "phone", traineeDto.Phone ?? "" },
                { "birthday", traineeDto.BirthDay.ToString("dd.MM.yyyy") },
                { "project", traineeDto.Project.Name },
                { "direction", traineeDto.Direction.Name }
            };
            await _hubContext.Clients.All.SendAsync("ReceiveCreate", notification);
            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("/trainees")]
        public async Task<ActionResult> EditAsync(
            int traineeId, string traineeName, string traineeSurname, 
            GenderDTO traineeGender, string traineeEmail, string traineePhone,
            DateOnly traineeBirthday)
        {
            var traineeDto = new TraineeDTO
            {
                Id = traineeId,
                Name = traineeName,
                Surname = traineeSurname,
                Gender = traineeGender,
                Email = traineeEmail,
                Phone = traineePhone,
                BirthDay = traineeBirthday,
            };
            string message = "Форма успешно отправлена";
            try
            {
                await _traineeService.Update(traineeDto);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            TempData["Message"] = message;
            var notification = new Dictionary<string, string>
            {
                { "id", traineeDto.Id.ToString() },
                { "name", traineeDto.Name },
                { "surname", traineeDto.Surname },
                { "gender", traineeDto.Gender.ToString() },
                { "email", traineeDto.Email },
                { "phone", traineeDto.Phone },
                { "birthday", traineeDto.BirthDay.ToString("dd.MM.yyyy") }
            };
            await _hubContext.Clients.All.SendAsync("ReceiveEdit", notification);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Route("/trainees-list")]
        public async Task<ActionResult> List(int directionId = -1, int projectId = -1)
        {
            var trainees = await _traineeService.GetAll();
            var directions = await _directionService.GetAll();
            var projects = await _projectService.GetAll();
            try
            {
                if (directionId != -1)
                {
                    trainees = _traineeService.FilterByDirections(trainees, directionId);
                }
                if (projectId != -1)
                {
                    trainees = _traineeService.FilterByProjects(trainees, projectId);
                }
            } catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            TraineeListViewModel model = new TraineeListViewModel
            {
                Trainees = trainees,
                Directions = directions,
                Projects = projects
            };
            return View(model);
        }
    }
}
