using BLL.DTO;
using WebApi.Models;

namespace BLL.Interfaces
{
    public interface IBaseService<ItemDTO>
    {
        public Task Create(ItemDTO itemDto);
        public Task Update(ItemDTO itemDto);
        public Task<ItemDTO> Delete(int id);
        public Task<IEnumerable<ItemDTO>> GetAll(
            SortingKey sortKey, bool descending,
            int index, int size, string? name);
        public Task<ItemDTO> Retrieve(int id);
        public Task<IEnumerable<ItemDTO>> GetAll();
    }
}
