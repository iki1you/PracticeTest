using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private ApplicationContext _db;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _db = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll(
            string includeProperties,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool descending = false,
            int page = 0, int pageSize = 10)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(property);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (descending)
                query = query.Reverse();

            query = query.Skip(page * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

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
            await _dbSet.AddAsync(entity);

        public async Task Update(TEntity entity) => 
            await Task.Run(() => { _db.Update(entity); });

        public async Task Delete(TEntity entity) =>
            await Task.Run(() => { _dbSet.Remove(entity); });
    }
}
