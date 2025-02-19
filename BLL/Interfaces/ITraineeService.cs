using BLL.DTO;
using WebApi.Models;

namespace BLL.Interfaces
{
    public interface ITraineeService: ICRUDableService<TraineeDTO>
    {
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
        public IEnumerable<TraineeDTO> GetByDirectionId(int Id);
    }
}
