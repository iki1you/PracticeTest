using BLL.DTO;
using BLL.Services.FuncSignatures;
using WebApi.Models;

namespace BLL.Interfaces
{
    public interface IBaseService<ItemDTO> where ItemDTO : class
    {
        public Task Create(ItemDTO itemDto);
        public Task Update(ItemDTO itemDto);
        public Task<ItemDTO> Delete(int id);
        public Task<ServicesGetAllReturn<ItemDTO>> GetAll(
            ServicesGetAllParameters<ItemDTO> dataParams);
        public Task<ItemDTO> Retrieve(int id);
        public Task<IEnumerable<ItemDTO>> GetAll();
    }
}
