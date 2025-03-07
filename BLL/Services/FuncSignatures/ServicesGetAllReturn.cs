using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FuncSignatures
{
    public class ServicesGetAllReturn<ItemDTO>(
        IEnumerable<ItemDTO> entities, int pageCount
        ) where ItemDTO : class
    {
        public IEnumerable<ItemDTO> Entities = entities;
        public int PageCount = pageCount;
    }
}
