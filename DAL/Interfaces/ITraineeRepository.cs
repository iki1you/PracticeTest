using DAL.Models;

namespace DAL.Interfaces
{
    public interface ITraineeRepository: ICRUDableRepository<Trainee>
    {
        public Trainee? Retrieve(string email);
    }
}
