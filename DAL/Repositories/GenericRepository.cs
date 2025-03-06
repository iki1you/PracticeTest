using DAL.EF;
using DAL.Interfaces;
using DAL.Repositories.FuncSignatures;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _db = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<GetAllReturn<TEntity>> GetAll(GetAllParameters<TEntity> dataParams)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var property in dataParams.IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(property);

            if (dataParams.Predicate != null)
                query = query.Where(dataParams.Predicate);

            if (dataParams.OrderBy != null)
                query = dataParams.OrderBy(query);

            if (dataParams.Descending)
                query = query.Reverse();

            var pageCount = await GetWithTotal(query, dataParams.PageSize);
            query = query.Skip(dataParams.Page * dataParams.PageSize).Take(dataParams.PageSize);

            return new GetAllReturn<TEntity>(
                await query.ToListAsync(),
                pageCount
                );
        }

        private static async Task<int> GetWithTotal(IQueryable<TEntity> query, int pageSize) => 
            (int)Math.Ceiling(await query.CountAsync() * 1.0 / pageSize);

        public async Task<TEntity?> Retrieve(Expression<Func<TEntity, bool>> predicate,
            string? includeProperties = null)
        {
            if (includeProperties == null)
                return await _dbSet.FirstOrDefaultAsync(predicate);

            IQueryable<TEntity> query = _dbSet;

            foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(property);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task Create(TEntity entity) =>
            await Task.FromResult(_dbSet.Add(entity));

        public async Task Update(TEntity entity) =>
            await Task.FromResult(_db.Update(entity));

        public async Task Delete(TEntity entity) =>
            await Task.FromResult(_dbSet.Remove(entity));
    }
}
