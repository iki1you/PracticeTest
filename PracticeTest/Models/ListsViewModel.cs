using BLL.DTO;

namespace WebApi.Models
{
    public class ListsViewModel
    {
        public IEnumerable<(DirectionDTO, IEnumerable<TraineeDTO>)> DirectionGroups { get; set; }
        public IEnumerable<(ProjectDTO, IEnumerable<TraineeDTO>)> ProjectGroups { get; set; }
        public PageListState PageListState { get; set; }
    }
}
