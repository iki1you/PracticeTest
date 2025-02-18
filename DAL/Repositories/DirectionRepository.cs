using DAL.EF;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DirectionRepository: ICRUDableRepository<Direction>, IDirectionRepository
    {
        private ApplicationContext _db;

        public DirectionRepository(ApplicationContext context)
        {
            _db = context;
        }

        public IEnumerable<Direction> GetAll()
        {
            return _db.Directions.ToList();
        }

        public Direction? Retrieve(int id)
        {
            return _db.Directions.Find(id);
        }

        public void Create(Direction direction)
        {
            _db.Add(direction);
            _db.SaveChanges();
        }

        public void Update(Direction direction)
        {
            _db.Update(direction);
            _db.SaveChanges();
        }

        public Direction? Delete(int id)
        {
            var direction = _db.Directions.Find(id);
            if (direction == null)
                return null;
            _db.Remove(id);
            _db.SaveChanges();
            return direction;
        }
    }
}
