using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.FuncSignatures
{
    public class ReposGetAllReturn<TEntity>(
        List<TEntity> entities, int pageCount
        ) where TEntity : class
    {
        public List<TEntity> Entities = entities;
        public int PageCount = pageCount;
    }
}
