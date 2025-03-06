using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

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
        // Лучше вынести параметры и кортеж в отдельные классы.
        public async Task<(IEnumerable<TEntity>, int)> GetAll(
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
            // Этого точно не должно быть в методе GetAll. Можно вынести в метод GetWithTotal.
            // Нужно использовать CountAsync, это тоже запрос к базе данных.
            int countPages = (int)Math.Ceiling((query.Count() * 1.0) / pageSize);
            query = query.Skip(page * pageSize).Take(pageSize);

            return (await query.ToListAsync(), countPages);
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
            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.addasync?view=efcore-9.0
            // This method is async only to allow special value generators, such as the one used by
            // 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
            // to access the database asynchronously. For all other cases the non async method should be used.
            await _dbSet.AddAsync(entity);

        public async Task Update(TEntity entity) =>
            // Нужно использовать Task.FromResult.
            // https://stackoverflow.com/questions/52146203/async-method-with-no-await-vs-task-fromresult
            // Task.Run передает работу в пул потоков, это можно использовать для CPU bound задач.
            // Здесь это приводит только к дополнительным накладным расходам
            // https://habr.com/ru/articles/470830/
            await Task.Run(() => { _db.Update(entity); });

        public async Task Delete(TEntity entity) =>
            await Task.Run(() => { _dbSet.Remove(entity); });
    }
}
