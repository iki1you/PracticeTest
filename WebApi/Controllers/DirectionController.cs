using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;
using WebApi.SignalRHubs;

namespace WebApi.Controllers
{
    public class DirectionController : Controller
    {
        private readonly IDirectionService _directionService;
        private readonly ITraineeService _traineeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public DirectionController(
            IDirectionService directionService,
            ITraineeService traineeService, 
            IHubContext<NotificationHub> hubContext)
        {
            _directionService = directionService;
            _traineeService = traineeService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Edit(
            int directionId, string directionName,
            int index, StateChoose choose, bool descending, int pageSize)
        {
            var directionDto = new DirectionDTO
            {
                Id = directionId,
                Name = directionName,
            };
            try
            {
                await _directionService.Update(directionDto);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists",
                new { index, choose, descending, pageSize });
        }

        public async Task<IActionResult> Delete(int directionId, 
            int index, StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                await _directionService.Delete(directionId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists",
                new { index, choose, descending, pageSize });
        }

        public async Task<IActionResult> AttachTraineeAsync(
            int traineeId, int directionId, int index,
            StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                var trainee = await _traineeService.Retrieve(traineeId);
                var direction = await _directionService.Retrieve(directionId);
                await _traineeService.AttachDirection(trainee, direction);
                var notification = new Dictionary<string, string>
                {
                    { "id", trainee.Id.ToString() },
                    { "name", trainee.Name },
                    { "surname", trainee.Surname },
                    { "gender", trainee.Gender.ToString() },
                    { "email", trainee.Email },
                    { "phone", trainee.Phone ?? "" },
                    { "birthday", trainee.BirthDay.ToString("dd.MM.yyyy") },
                    { "project", trainee.Project.Name },
                    { "direction", direction.Name }
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
