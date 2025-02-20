using BLL.DTO;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class DirectionController : Controller
    {
        private readonly IDirectionService _directionService;
        private readonly ITraineeService _traineeService;
        public DirectionController(
            IDirectionService directionService,
            ITraineeService traineeService)
        {
            _directionService = directionService;
            _traineeService = traineeService;
        }

        public IActionResult Edit(
            int directionId, string directionName, int directionTrainees,
            int index, StateChoose choose, bool descending, int pageSize)
        {
            var directionDto = new DirectionDTO
            {
                Id = directionId,
                Name = directionName,
                TraineeCount = directionTrainees
            };
            try
            {
                _directionService.Update(directionDto);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists",
                new { index, choose, descending, pageSize });
        }

        public IActionResult Delete(int directionId, 
            int index, StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                _directionService.Delete(directionId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists",
                new { index, choose, descending, pageSize });
        }

        public IActionResult AttachTrainee(
            int traineeId, int directionId, int index,
            StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                var trainee = _traineeService.Retrieve(_traineeService.GetAll(), traineeId);
                var direction = _directionService.Retrieve(directionId);
                _traineeService.AttachDirection(trainee, direction);
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
