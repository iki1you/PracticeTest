using DAL.EF;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class UnitOfWork(ApplicationContext context) : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context = context;
        private GenericRepository<Direction>? _directionRepository;
        private GenericRepository<Project>? _projectRepository;
        private GenericRepository<Trainee>? _traineeRepository;

        public GenericRepository<Direction> Directions
        {
            get { return _directionRepository ??= new GenericRepository<Direction>(_context); }
        }

        public GenericRepository<Project> Projects
        {
            get { return _projectRepository ??= new GenericRepository<Project>(_context); }
        }

        public GenericRepository<Trainee> Trainees
        {
            get { return _traineeRepository ??= new GenericRepository<Trainee>(_context); }
        }

        public async Task Save() => await _context.SaveChangesAsync();

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
