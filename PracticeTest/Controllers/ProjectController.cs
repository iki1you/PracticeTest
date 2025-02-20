using BLL.DTO;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PracticeTest.Controllers;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ITraineeService _traineeService;
        public ProjectController(
            IProjectService projectService,
            ITraineeService traineeService)
        {
            _projectService = projectService;
            _traineeService = traineeService;
        }

        public IActionResult Edit(
            int projectId, string projectName, int projectTrainees,
            int index, StateChoose choose, bool descending, int pageSize)
        {
            var projectDto = new ProjectDTO
            {
                Id = projectId,
                Name = projectName,
                TraineeCount = projectTrainees
            };
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

        public IActionResult Delete(
            int projectId, int index, StateChoose choose, 
            bool descending, int pageSize)
        {
            try
            {
                _projectService.Delete(projectId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Lists", 
                new { index, choose, descending, pageSize });
        }
        public IActionResult AttachTrainee(
            int traineeId, int projectId, int index, 
            StateChoose choose, bool descending, int pageSize)
        {
            try
            {
                var trainee = _traineeService.Retrieve(_traineeService.GetAll(), traineeId);
                var project = _projectService.Retrieve(projectId);
                _traineeService.AttachProject(trainee, project);
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
