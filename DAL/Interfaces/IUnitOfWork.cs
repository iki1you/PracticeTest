using DAL.Models;
using DAL.Repositories;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        public GenericRepository<Direction> Directions { get; }
        public GenericRepository<Project> Projects { get; }
        public GenericRepository<Trainee> Trainees { get; }
        public Task Save();
    }
}
