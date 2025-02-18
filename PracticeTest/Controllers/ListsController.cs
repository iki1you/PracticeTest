using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracticeTest.Controllers;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ListsController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        private readonly ILogger<HomeController> _logger;
        public ListsController(ITraineeService traineeService,
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

        public IActionResult Index(
            string searchName, SortingKeys sortbyName, bool descending, 
            int directionIndex = 0, int projectIndex = 0, int pageSize = 10)
        {
            var directions = _directionService.GetAll();
            var projects = _projectService.GetAll();
            var trainees = _traineeService.GetAll();

            if (searchName != null)
            {
                directions = _directionService.FindByName(directions, searchName);
                projects = _projectService.FindByName(projects, searchName);
            }

            switch (sortbyName)
            {
                case SortingKeys.Name:
                    directions = _directionService.GetSortedByName(directions, descending);
                    projects = _projectService.GetSortedByName(projects, descending);
                    break;
                case SortingKeys.TraineeCount:
                    directions = _directionService.GetSortedByTrainees(directions, descending);
                    projects = _projectService.GetSortedByTrainees(projects, descending);
                    break;
                default:
                    break;
            }

            var directionIndexMax = Math.Ceiling(1.0 * directions.Count() / pageSize);
            var projectIndexMax = Math.Ceiling(1.0 * projects.Count() / pageSize);

            directions = _directionService.GetRangeDirections(directions, directionIndex, pageSize);
            projects = _projectService.GetRangeProjects(projects, projectIndex, pageSize);

            
            trainees = _traineeService.FilterByProjects(trainees, projects);
            trainees = _traineeService.FilterByDirections(trainees, directions);

            var traineesByProjects = _traineeService.GroupByProjects(trainees);
            var traineesByDirections = _traineeService.GroupByDirections(trainees);
            switch (sortbyName)
            {
                case SortingKeys.Name:
                    traineesByProjects = traineesByProjects.OrderBy(x => x.Key.Name);
                    traineesByDirections = traineesByDirections.OrderBy(x => x.Key.Name);
                    break;
                case SortingKeys.TraineeCount:
                    traineesByProjects = traineesByProjects.OrderBy(x => x.Key.TraineeCount);
                    traineesByDirections = traineesByDirections.OrderBy(x => x.Key.TraineeCount);
                    break;
                default:
                    break;
            }
            ViewBag.TraineeProjects = traineesByProjects;
            ViewBag.TraineeDirections = traineesByDirections;
            return View();
        }
    }
}
