using DAL.Repositories.FuncSignatures;
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
        public Task<GetAllReturn<TEntity>> GetAll(GetAllParameters<TEntity> dataParams);
        public Task Delete(TEntity entity);
        public Task Update(TEntity item);
    }
}
