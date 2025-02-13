using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(ITraineeRepository traineeRepository,
            IDirectionRepository directionRepository,
            IProjectRepository projectRepository)
        {
            _traineeRepository = traineeRepository;
            _projectRepository = projectRepository;
        }

        public void Create(ProjectDTO project)
        {
            _projectRepository.Create(new Project {
                Name = project.Name
            });
        }

        public ProjectDTO Delete(Guid id)
        {
            var projectDto = Retrieve(id);

            var trainees = _traineeRepository.GetAll().Where(x => x.Project.Id == id);
            if (trainees.Any())
                throw new ArgumentException($"You can't delete, {trainees} subscribes to this project");

            _projectRepository.Delete(id);
            return projectDto;
        }

        public ProjectDTO Retrieve(Guid id)
        {
            var project = _projectRepository.Retrieve(id);
            if (project == null)
                throw new ArgumentNullException("Project doesn`t exist");
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
            };
        }

        public void Update(ProjectDTO projectDto)
        {
            var project = _projectRepository.Retrieve(projectDto.Id);
            if (project == null)
                throw new ArgumentNullException("Project doesn`t exist");
            _projectRepository.Update(project);
        }

        public IEnumerable<Project> GetAll()
        {
            return _projectRepository.GetAll();
        }
    }
}
