using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICRUDableService<ItemDTO>
    {
        public void Create(ItemDTO itemDto);
        public ItemDTO Retrieve(int id);
        public void Update(ItemDTO itemDto);
        public ItemDTO Delete(int id);
        public IEnumerable<ItemDTO> GetAll();
    }
}
