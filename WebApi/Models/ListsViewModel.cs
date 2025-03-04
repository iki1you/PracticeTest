using BLL.DTO;

namespace WebApi.Models
{
    public class ListsViewModel
    {
        public IEnumerable<DirectionDTO> Directions { get; set; }
        public IEnumerable<ProjectDTO> Projects { get; set; }
        public PageListState PageListState { get; set; }
        public ProjectDTO Project { get; set; }
        public DirectionDTO Direction { get; set; }
        public IEnumerable<TraineeDTO> Trainees { get; set; }
    }
}
