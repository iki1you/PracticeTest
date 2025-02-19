using WebApi.Models;

namespace BLL.Interfaces
{
    public interface IFormattableService<ItemDTO>
    {
        public IEnumerable<ItemDTO> GetSorted(
            IEnumerable<ItemDTO> directionDTOs, SortingKey sortingKey, bool descending);
        public IEnumerable<ItemDTO> GetRange(
            IEnumerable<ItemDTO> directionDTOs, int index, int size);
        public IEnumerable<ItemDTO> FindByName(
            IEnumerable<ItemDTO> directions, string name);
    }
}
