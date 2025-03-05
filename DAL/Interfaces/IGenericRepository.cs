using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task Create(TEntity item);
        public Task<TEntity?> Retrieve(
            Expression<Func<TEntity, bool>> predicate, string? includeProperties);
        public Task<(IEnumerable<TEntity>, int)> GetAll(
           string includeProperties,
           Expression<Func<TEntity, bool>>? predicate,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
           bool descending,
           int page, int pageSize);
        public Task Delete(TEntity entity);
        public Task Update(TEntity item);
    }
}
