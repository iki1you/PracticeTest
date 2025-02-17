using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracticeTest.Controllers;
using System.ComponentModel;

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
            ViewBag.Projects = new SelectList(_projectService.GetAll());
            ViewBag.Directions = new SelectList(_directionService.GetAll());
            return View();
        }

        [HttpPost]
        [Route("/create")]
        public ActionResult Create(TraineeDTO traineeDto)
        {
            string message = "Форма успешно отправлена";
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ValidationState.ToString());
                ViewData["Message"] = "Невалидная форма";
                return View("Create");
            }
            try
            {
                _traineeService.Create(traineeDto);
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            Console.WriteLine(message);

            return RedirectToAction("Create", (object)message);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
