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
    public class DirectionRepository: ICRUDable<Direction>
    {
        private ApplicationContext _db;

        public DirectionRepository(ApplicationContext context)
        {
            _db = context;
        }

        public IEnumerable<Direction> GetAll()
        {
            return _db.Directions;
        }

        public Direction? Retrieve(Guid id)
        {
            return _db.Directions.Find(id);
        }

        public void Create(Direction direction)
        {
            _db.Add(direction);
        }

        public void Update(Direction direction)
        {
            _db.Update(direction);
            _db.SaveChanges();
        }

        public Direction? Delete(Guid id)
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
