using AutoMapper;
using BLL.DTO;
using BLL.Exeptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.Linq.Expressions;
using WebApi.Models;

namespace BLL.Services
{
    public class DirectionService: IDirectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DirectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(DirectionDTO direction)
        {
            var proj = await _unitOfWork.Directions.Retrieve(x => x.Name == direction.Name);
            if (proj != null)
                throw new DirectionNotFoundException("This direction already exists");
            await _unitOfWork.Directions.Create(_mapper.Map<Direction>(direction));
            await _unitOfWork.Save();
        }

        public async Task<DirectionDTO> Delete(int id)
        {
            var directionDto = await Retrieve(id);

            if (directionDto.Trainees.Count() > 0)
                throw new ArgumentException($"Can`t delete, unlink all trainees from the direction");
            
            await _unitOfWork.Directions.Delete(_mapper.Map<Direction>(directionDto));
            await _unitOfWork.Save();
            return directionDto;
        }

        public async Task<DirectionDTO> Retrieve(int id)
        {
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == id, "Trainees");
            if (direction == null)
                throw new DirectionNotFoundException("This direction doesn`t exist");
            return _mapper.Map<DirectionDTO>(direction);
        }

        public async Task Update(DirectionDTO directionDto)
        {
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == directionDto.Id, "Trainees");
            if (direction == null)
                throw new DirectionNotFoundException("This direction doesn`t exist");
            if (await _unitOfWork.Directions.Retrieve(x => x.Name == directionDto.Name, "Trainees") != null)
                throw new DirectionNotFoundException("Direction with this name exists");
            direction.Name = directionDto.Name;
            await _unitOfWork.Directions.Update(direction);
            await _unitOfWork.Save();
        }

        public async Task<(IEnumerable<DirectionDTO>, int)> GetAll(
            SortingKey sortKey, bool descending=false, 
            int index=0, int size=10, string? name=null)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");

            Expression<Func<Direction, bool>>? predicate = null;
            if (name != null)
                predicate = d => d.Name == name;

            Func<IQueryable<Direction>, IOrderedQueryable<Direction>>? orderBy = null;
            switch (sortKey)
            {
                case SortingKey.Name:
                    orderBy = q => q.OrderBy(d => d.Name);
                    break;
                case SortingKey.TraineeCount:
                    orderBy = q => q.OrderBy(d => d.Trainees.Count);
                    break;
                default:
                    break;
            }

            var directions = await _unitOfWork.Directions.GetAll(
                "Trainees", predicate, orderBy, descending, index, size);
            return (directions.Item1.Select(x => _mapper.Map<DirectionDTO>(x)), directions.Item2);
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            var directions = await _unitOfWork.Directions.GetAll("Trainees");
            return directions.Item1.Select(x => _mapper.Map<DirectionDTO>(x));
        }
    }
}
