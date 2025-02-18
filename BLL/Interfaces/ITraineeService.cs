using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITraineeService: ICRUDableService<TraineeDTO>
    {
        public void AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto);
        public void AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto);
        public IEnumerable<IGrouping<ProjectDTO, TraineeDTO>> GroupByProjects(
            IEnumerable<TraineeDTO> traineesDto);
        public IEnumerable<IGrouping<DirectionDTO, TraineeDTO>> GroupByDirections
            (IEnumerable<TraineeDTO> traineesDto);
        public IEnumerable<TraineeDTO> FilterByDirections(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<DirectionDTO> directions);
        public IEnumerable<TraineeDTO> FilterByProjects(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<ProjectDTO> projects);
        public IEnumerable<TraineeDTO> GetByDirectionId(int Id);
    }
}
