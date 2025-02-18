using DAL.Models;

namespace DAL.Interfaces
{
    public interface ITraineeRepository: ICRUDable<Trainee>
    {
        public Trainee? Retrieve(string email);
    }
}
