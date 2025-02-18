using BLL.DTO;

namespace WebApi.Models
{
    public class ListsViewModel
    {
        public IEnumerable<IGrouping<DirectionDTO, TraineeDTO>> DirectionGroups { get; set; }
        public IEnumerable<IGrouping<ProjectDTO, TraineeDTO>> ProjectGroups { get; set; }
    }
}
