using BLL.Interfaces;
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
                // Пагинацию нужно делать на уровне бд, загрузка большой таблицы целиком будет долгой и запросто положит сервер.
                // Фильтрации и сортировки также нужно вынести на бд.
                var directions = await _directionService.GetAll(sortOrder, descending, index, pageSize, searchName);    
                pageState.PageMax = (int)Math.Ceiling(1.0 * directions.Count() / pageSize);
                model = new ListsViewModel
                {
                    Directions = directions,
                    PageListState = pageState,
                    Trainees = trainees
                };
            } else
            {
                var projects = await _projectService.GetAll(sortOrder, descending, index, pageSize, searchName);
                pageState.PageMax = (int)Math.Ceiling(1.0 * projects.Count() / pageSize);
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
