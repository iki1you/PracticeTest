using BLL.DTO;

namespace WebApi.Models
{
    public class TraineeListViewModel
    {
        public IEnumerable<TraineeDTO> Trainees { get; set; }
        public IEnumerable<DirectionDTO> Directions { get; set; }
        public IEnumerable<ProjectDTO> Projects { get; set; }
        public TraineeDTO Trainee { get; set; }
    }
}
