using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITraineeService: IBaseService<TraineeDTO>
    {
        public Task AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto);
        public Task AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto);
        public Task<TraineeDTO> Retrieve(string email);
        public IEnumerable<TraineeDTO> FilterByDirections(
            IEnumerable<TraineeDTO> traineeDTOs, int directionId);
        public IEnumerable<TraineeDTO> FilterByProjects(
            IEnumerable<TraineeDTO> traineeDTOs, int projectId);
    }
}
