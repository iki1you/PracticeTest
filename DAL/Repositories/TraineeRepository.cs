using DAL.EF;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TraineeRepository : ITraineeRepository, ICRUDableRepository<Trainee>
    {
        private ApplicationContext _db;

        public TraineeRepository(ApplicationContext context)
        {
            _db = context;
        }

        public IEnumerable<Trainee> GetAll()
        {
            return _db.Trainees.Include(x => x.Project).Include(x => x.Direction);
        }

        public Trainee? Retrieve(int id)
        {
            return _db.Trainees.FirstOrDefault(x => x.Id == id);
        }
        public Trainee? Retrieve(string email)
        {
            return _db.Trainees.FirstOrDefault(x => x.Email == email);
        }

        public void Create(Trainee trainee)
        {
            _db.Add(trainee);
            _db.SaveChanges();
        }

        public void Update(Trainee trainee)
        {
            _db.Update(trainee);
            _db.SaveChanges();
        }

        public Trainee? Delete(int id)
        {
            var trainee = _db.Trainees.Find(id);
            if (trainee == null)
                return null;
            _db.Remove(id);
            _db.SaveChanges();
            return trainee;
        }
    }
}
