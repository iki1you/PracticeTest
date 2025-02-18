using BLL.DTO;

namespace WebApi.Models
{
    public class CreateViewModel
    {
        public TraineeDTO Trainee { get; set; }
        public ProjectDTO Project { get; set; }
        public DirectionDTO Direction { get; set; }
    }
}
