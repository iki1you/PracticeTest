using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace BLL.Services.FuncSignatures
{
    public class ServicesGetAllParameters<ItemDTO> where ItemDTO : class
    {
        public SortingKey SortKey;
        public bool Descending = false;
        public int Index = 0;
        public int Size = 10;
        public string? Name = null;

        public ServicesGetAllParameters(
            SortingKey sortKey, bool descending = false,
            int index = 0, int size = 10, string? name = null)
        {
            SortKey = sortKey;
            Descending = descending;
            Index = index;
            Size = size;
            Name = name;
        }
    }
}
