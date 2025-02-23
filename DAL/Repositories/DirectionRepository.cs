using DAL.EF;
using DAL.Interfaces;
using DAL.Models;
namespace DAL.Repositories
{
    public class DirectionRepository: ICRUDableRepository<Direction>, IDirectionRepository
    {
        private ApplicationContext _db;

        public DirectionRepository(ApplicationContext context)
        {
            _db = context;
        }

        public IEnumerable<Direction> GetAll() => _db.Directions.ToList();

        public Direction? Retrieve(int id) => _db.Directions.FirstOrDefault(x => x.Id == id);

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
            _db.Remove(direction);
            _db.SaveChanges();
            return direction;
        }
    }
}
