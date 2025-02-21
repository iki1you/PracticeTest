using BLL.DTO;
using WebApi.Models;

namespace BLL.Interfaces
{
    public interface ITraineeService
    {
        public void Create(TraineeDTO itemDto);
        public void Update(TraineeDTO itemDto);
        public IEnumerable<TraineeDTO> GetAll();
        public void AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto);
        public void AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto);
        public IEnumerable<(ProjectDTO, IEnumerable<TraineeDTO>)> GroupByProjects(
            IEnumerable<ProjectDTO> projectsDto,
            IEnumerable<TraineeDTO> traineesDto,
            SortingKey sortKey, bool descending);
        public IEnumerable<(DirectionDTO, IEnumerable<TraineeDTO>)> GroupByDirections(
            IEnumerable<DirectionDTO> directionsDto,
            IEnumerable<TraineeDTO> traineesDto,
            SortingKey sortKey, bool descending);
        public IEnumerable<TraineeDTO> FilterByDirections(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<DirectionDTO> directions);
        public IEnumerable<TraineeDTO> FilterByProjects(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<ProjectDTO> projects);
        public TraineeDTO Retrieve(IEnumerable<TraineeDTO> items, int id);
        public TraineeDTO Retrieve(IEnumerable<TraineeDTO> items, string email);
    }
}
