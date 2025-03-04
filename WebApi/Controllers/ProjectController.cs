using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;
using WebApi.SignalRHubs;

namespace WebApi.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ITraineeService _traineeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ProjectController(
            IProjectService projectService,
            ITraineeService traineeService,
            IHubContext<NotificationHub> hubContext)
        {
            _projectService = projectService;
            _traineeService = traineeService;
            _hubContext = hubContext;
        }

        public IActionResult Edit(
            int projectId, string projectName, int projectTrainees,
            int index, StateChoose choose, bool descending, int pageSize)
        {
            var projectDto = new ProjectDTO
            {
                Id = projectId,
                Name = projectName
            };
            // Можно вынести в глобальный обработчик ошибок
            try
            {
                _projectService.Update(projectDto);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists", 
                new { index, choose, descending, pageSize });
        }

        public async Task<IActionResult> Delete(
            int projectId, int index, StateChoose choose, 
            bool descending, int pageSize)
        {
            try
            {
                await _projectService.Delete(projectId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists", 
                new { index, choose, descending, pageSize });
        }
        public async Task<IActionResult> AttachTraineeAsync(
            int traineeId, int projectId, int index, 
            StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                var trainee = await _traineeService.Retrieve(traineeId);
                var project = await _projectService.Retrieve(projectId);
                await _traineeService.AttachProject(trainee, project);
                // Для уведомления нужно использовать DTO и маппер
                var notification = new Dictionary<string, string>
                {
                    { "id", trainee.Id.ToString() },
                    { "name", trainee.Name },
                    { "surname", trainee.Surname },
                    { "gender", trainee.Gender.ToString() },
                    { "email", trainee.Email },
                    { "phone", trainee.Phone ?? "" },
                    { "birthday", trainee.BirthDay.ToString("dd.MM.yyyy") },
                    { "project", project.Name },
                    { "direction", trainee.Project.Name }
                };
                await _hubContext.Clients.All.SendAsync("ReceiveEdit", notification);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists",
                new { index, choose, descending, pageSize });
        }
    }
}
