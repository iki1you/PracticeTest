using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.FuncSignatures
{
    public class GetAllParameters<TEntity>
    {
        public string IncludeProperties = "";
        public Expression<Func<TEntity, bool>>? Predicate = null;
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy = null;
        public bool Descending = false;
        public int Page = 0;
        public int PageSize = 10;

        public GetAllParameters(
            string includeProperties,
            Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
            bool descending, int page, int pageSize)
        {
            IncludeProperties = includeProperties;
            Predicate = predicate;
            OrderBy = orderBy;
            Descending = descending;
            Page = page;
            PageSize = pageSize;
        }
    }
}
