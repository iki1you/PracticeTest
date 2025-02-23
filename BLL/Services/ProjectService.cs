using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using WebApi.Models;

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
            if (_projectRepository.GetAll().Any( x => x.Name == project.Name ))
                throw new ArgumentException("Проект с таким именем существует");
            _projectRepository.Create(new Project {
                Name = project.Name
            });
        }

        public ProjectDTO Delete(int id)
        {
            var projectDto = Retrieve(id);

            var trainees = _traineeRepository
                .GetAll().Where(x => x.Project.Id == id);
            if (trainees.Any())
                throw new ArgumentException($"Нельзя удалить, отвяжите всех стажеров от проекта");

            _projectRepository.Delete(id);
            return projectDto;
        }

        public ProjectDTO Retrieve(int id)
        {
            var project = _projectRepository.Retrieve(id);
            if (project == null)
                throw new ArgumentNullException("Такого проекта не существует");
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                TraineeCount = project.TraineeCount
            };
        }

        public void Update(ProjectDTO projectDto)
        {
            var project = _projectRepository.Retrieve(projectDto.Id);
            if (project == null)
                throw new ArgumentNullException("Такого проекта не существует");
            project.Name = projectDto.Name;
            project.TraineeCount = projectDto.TraineeCount;
            _projectRepository.Update(project);
        }

        public IEnumerable<ProjectDTO> GetAll() => 
            _projectRepository.GetAll().Select(x => new ProjectDTO
            {
                Id = x.Id,
                Name = x.Name,
                TraineeCount = x.TraineeCount
            });


        public IEnumerable<ProjectDTO> GetSorted(
            IEnumerable<ProjectDTO> projectDTOs, SortingKey sortKey, bool descending)
        {
            var sorted = projectDTOs;
            switch (sortKey)
            {
                case SortingKey.Name:
                    sorted = projectDTOs.OrderBy(x => x.Name);
                    break;
                case SortingKey.TraineeCount:
                    sorted = projectDTOs.OrderBy(x => x.TraineeCount);
                    break;
                default:
                    break;
            }
            return descending ? sorted.Reverse() : sorted;
        }

        public IEnumerable<ProjectDTO> GetRange(
            IEnumerable<ProjectDTO> projectDTOs, int index, int size)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");
            return projectDTOs.Skip(index * size).Take(size);
        }

        public IEnumerable<ProjectDTO> FindByName(
            IEnumerable<ProjectDTO> projects, string name) => projects
                .Where(project => project.Name.Contains(name))
                .Select(project => Retrieve(project.Id));
    }
}
