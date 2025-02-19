using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
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
            string searchName, bool descending, 
            int index, int pageSize = 10, 
            SortingKey sortOrder = SortingKey.Name, 
            StateChoose choose = StateChoose.Directions)
        {  
            var trainees = _traineeService.GetAll();
            ListsViewModel model;
            var pageState = new PageListState
            {
                SearchName = searchName,
                Descending = descending,
                CurrentPage = index,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Choose = choose
            };
            if (choose == StateChoose.Directions)
            {
                var directions = _directionService.GetAll();
                if (searchName != null)
                {
                    directions = _directionService.FindByName(directions, searchName);
                }
                pageState.PageMax = (int)Math.Ceiling(1.0 * directions.Count() / pageSize);
                directions = _directionService.GetSorted(directions, sortOrder, descending);
                directions = _directionService.GetRange(directions, index, pageSize);
                var traineesByDirections = _traineeService.GroupByDirections(
                    directions, trainees, sortOrder, descending);
                model = new ListsViewModel
                {
                    DirectionGroups = traineesByDirections,
                    PageListState = pageState
                };
            } else
            {
                var projects = _projectService.GetAll();
                if (searchName != null)
                {
                    projects = _projectService.FindByName(projects, searchName);
                }
                pageState.PageMax = (int)Math.Ceiling(1.0 * projects.Count() / pageSize);
                projects = _projectService.GetSorted(projects, sortOrder, descending);
                projects = _projectService.GetRange(projects, index, pageSize);
                var traineesByProjects = _traineeService.GroupByProjects(
                    projects, trainees, sortOrder, descending);
                model = new ListsViewModel
                {
                    ProjectGroups = traineesByProjects,
                    PageListState = pageState,
                };
            }
            
            return View(model);
        }
    }
}
