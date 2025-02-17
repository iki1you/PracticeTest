using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITraineeService: ICRUDableService<TraineeDTO>
    {
        public void AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto);
        public void AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto);
        public IEnumerable<TraineeDTO> GetByProjectId(int Id);
        public IEnumerable<TraineeDTO> GetByDirectionId(int Id);
    }
}
