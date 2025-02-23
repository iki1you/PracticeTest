using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using WebApi.Models;

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

            var trainees = _traineeRepository
                .GetAll().Where(x => x.Direction.Id == id).Select(x => x.Name);
            if (trainees.Any())
                throw new ArgumentException($"Нельзя удалить, отвяжите всех стажеров от направления");

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
            direction.Name = directionDto.Name;
            direction.TraineeCount = direction.TraineeCount;
            _directionRepository.Update(direction);
        }

        public IEnumerable<DirectionDTO> GetAll() => 
            _directionRepository.GetAll().Select(x => new DirectionDTO
            {
                Id = x.Id,
                Name = x.Name,
                TraineeCount = x.TraineeCount
            });


        public IEnumerable<DirectionDTO> GetSorted(
            IEnumerable<DirectionDTO> directionDTOs, SortingKey sortKey, bool descending)
        {
            var sorted = directionDTOs;
            switch (sortKey)
            {
                case SortingKey.Name:
                    sorted = directionDTOs.OrderBy(x => x.Name);
                    break;
                case SortingKey.TraineeCount:
                    sorted = directionDTOs.OrderBy(x => x.TraineeCount);
                    break;
                default:
                    break;
            }
            return descending ? sorted.Reverse() : sorted;
        }

        public IEnumerable<DirectionDTO> GetRange(
            IEnumerable<DirectionDTO> directionDTOs, int index, int size)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");
            return directionDTOs.Skip(index * size).Take(size);
        }

        public IEnumerable<DirectionDTO> FindByName(
            IEnumerable<DirectionDTO> directions, string name) => 
                directions.Where(direction => direction.Name.Contains(name))
                          .Select(direction => Retrieve(direction.Id));

    }
}
