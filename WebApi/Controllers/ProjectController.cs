using AutoMapper;
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
        private readonly IMapper _mapper;
        public ProjectController(
            IProjectService projectService,
            ITraineeService traineeService,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper)
        {
            _projectService = projectService;
            _traineeService = traineeService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Edit(
            int projectId, string projectName, int projectTrainees,
            int index, StateChoose choose, bool descending, int pageSize)
        {
            var projectDto = new ProjectDTO
            {
                Id = projectId,
                Name = projectName
            };
            try
            {
                await _projectService.Update(projectDto);
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
                trainee.Project = project;
                var notification = _mapper.Map<Dictionary<string, string>>(trainee);
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
