using BLL.DTO;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracticeTest.Controllers;
using System.Collections;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TraineeController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        private readonly ILogger<HomeController> _logger;
        public TraineeController(ITraineeService traineeService, 
            IProjectService projectService, 
            IDirectionService directionService,
            ILogger<HomeController> logger
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
            _logger = logger;

        }

        [HttpGet]
        [Route("/create")]
        public ActionResult Create()
        {
            ViewBag.Projects = new SelectList(_projectService.GetAll(), "Id", "Name");
            ViewBag.Directions = new SelectList(_directionService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateProject(CreateViewModel model)
        {
            if (model.Project != null)
            {
                try
                {
                    _projectService.Create(model.Project);
                    TempData["Message"] = "Проект создан";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Data;
                }
            }     
            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult CreateDirection(CreateViewModel model)
        {
            if (model.Direction != null)
            {
                try
                {
                    _directionService.Create(model.Direction);
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
        [Route("/create")]
        public ActionResult Create(CreateViewModel model)
        {
            var traineeDto = model.Trainee;
            traineeDto.Project = model.Project;
            traineeDto.Direction = model.Direction;
            string message = "Форма успешно отправлена";
            try
            {
                _traineeService.Create(traineeDto);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            TempData["Message"] = message;
            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("/trainees")]
        public ActionResult Edit(
            int traineeId, string traineeName, string traineeSurname, 
            Gender traineeGender, string traineeEmail, string traineePhone,
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
                _traineeService.Update(traineeDto);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            TempData["Message"] = message;
            return RedirectToAction("List");
        }

        [HttpGet]
        [Route("/trainees")]
        public ActionResult List(int directionId = -1, int projectId = -1)
        {
            var trainees = _traineeService.GetAll();
            var directions = _directionService.GetAll();
            var projects = _projectService.GetAll();
            try
            {
                if (directionId != -1)
                {
                    var direction = _directionService.Retrieve(directionId);
                    trainees = _traineeService.FilterByDirections(trainees,
                        new List<DirectionDTO> { direction });
                }
                if (projectId != -1)
                {
                    var project = _projectService.Retrieve(projectId);
                    trainees = _traineeService.FilterByProjects(trainees,
                        new List<ProjectDTO> { project });
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
