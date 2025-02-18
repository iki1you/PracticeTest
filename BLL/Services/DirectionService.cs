using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class DirectionService: IDirectionService
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IDirectionRepository _directionRepository;

        public DirectionService(ITraineeRepository traineeRepository,
            IDirectionRepository directionRepository)
        {
            _traineeRepository = traineeRepository;
            _directionRepository = directionRepository;
        }

        public void Create(DirectionDTO direction)
        {
            if (_directionRepository.GetAll().Any(x => x.Name == direction.Name))
                throw new ArgumentException("Такое направление уже существует");
            _directionRepository.Create(new Direction
            {
                Name = direction.Name
            });
        }

        public DirectionDTO Delete(int id)
        {
            var directionDto = Retrieve(id);

            var trainees = _traineeRepository.GetAll().Where(x => x.Direction.Id == id);
            if (trainees.Any())
                throw new ArgumentException($"Нельзя удалить, {trainees} подписаны на направление");

            _directionRepository.Delete(id);
            return directionDto;
        }

        public DirectionDTO Retrieve(int id)
        {
            var direction = _directionRepository.Retrieve(id);
            if (direction == null)
                throw new ArgumentNullException("Такое направление не существует");
            return new DirectionDTO
            {
                Id = direction.Id,
                Name = direction.Name,
                TraineeCount = direction.TraineeCount
            };
        }

        public void Update(DirectionDTO directionDto)
        {
            var direction = _directionRepository.Retrieve(directionDto.Id);
            if (direction == null)
                throw new ArgumentNullException("Такое направление не существует");
            _directionRepository.Update(new Direction
            {
                Id = directionDto.Id,
                Name = directionDto.Name,
                TraineeCount = direction.TraineeCount
            });
        }

        public IEnumerable<DirectionDTO> GetAll()
        {
            return _directionRepository.GetAll()
                .Select(x => new DirectionDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    TraineeCount = x.TraineeCount
                });
        }

        public IEnumerable<DirectionDTO> GetSortedByTrainees(IEnumerable<DirectionDTO> directionDTOs, bool descending)
        {
            var sorted = directionDTOs.OrderBy(x => x.TraineeCount);
            return descending? sorted.Reverse(): sorted;
        }

        public IEnumerable<DirectionDTO> GetSortedByName(IEnumerable<DirectionDTO> directionDTOs, bool descending)
        {
            var sorted = directionDTOs.OrderBy(x => x.Name);
            return descending ? sorted.Reverse() : sorted;
        }

        public IEnumerable<DirectionDTO> GetRangeDirections(IEnumerable<DirectionDTO> directionDTOs, int index, int size)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");
            return directionDTOs.Skip(index * size).Take(size);
        }

        public IEnumerable<DirectionDTO> FindByName(IEnumerable<DirectionDTO> directions, string name)
        {
            return directions
                .Where(direction => direction.Name.Contains(name))
                .Select(direction => Retrieve(direction.Id));
        }
    }
}
