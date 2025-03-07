using BLL.DTO;
using BLL.Interfaces;
using BLL.Services.FuncSignatures;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ListsController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IProjectService _projectService;
        private readonly IDirectionService _directionService;
        public ListsController(ITraineeService traineeService,
            IProjectService projectService,
            IDirectionService directionService
            )
        {
            _traineeService = traineeService;
            _projectService = projectService;
            _directionService = directionService;
        }

        [HttpGet]
        [Route("/lists-dir-prj")]
        public async Task<IActionResult> Index(
            string searchName, bool descending, 
            int index, int pageSize = 10, 
            SortingKey sortOrder = SortingKey.Name, 
            StateChoose choose = StateChoose.Directions)
        {  
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
            var trainees = await _traineeService.GetAll();
            if (choose == StateChoose.Directions)
            {
                var directionsGet = await _directionService.GetAll(
                    new ServicesGetAllParameters<DirectionDTO>(sortOrder, descending, index, pageSize, searchName));
                var directions = directionsGet.Entities;    
                pageState.PageMax = directionsGet.PageCount;
                model = new ListsViewModel
                {
                    Directions = directions,
                    PageListState = pageState,
                    Trainees = trainees
                };
            } else
            {
                var projectsGet = await _projectService.GetAll(
                    new ServicesGetAllParameters<ProjectDTO>(sortOrder, descending, index, pageSize, searchName));
                var projects = projectsGet.Entities;
                pageState.PageMax = projectsGet.PageCount;
                model = new ListsViewModel
                {
                    Projects = projects,
                    PageListState = pageState,
                    Trainees = trainees
                };
            }
            
            return View(model);
        }
    }
}
