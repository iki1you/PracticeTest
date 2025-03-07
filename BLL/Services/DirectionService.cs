using AutoMapper;
using BLL.DTO;
using BLL.Exeptions;
using BLL.Interfaces;
using BLL.Services.FuncSignatures;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.FuncSignatures;
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
            var proj = await _unitOfWork.Directions.Retrieve(x => x.Name == direction.Name, "Trainees");
            if (proj != null)
                throw new DataDuplicateExeption<Direction>("This direction already exists");
            await _unitOfWork.Directions.Create(_mapper.Map<Direction>(direction));
            await _unitOfWork.Save();
        }

        public async Task<DirectionDTO> Delete(int id)
        {
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == id, "Trainees");

            if (direction == null)
                throw new DataNotFoundExeption<Direction>("This direction doesn`t exist");
            if (direction.Trainees.Count() > 0)
                throw new ArgumentException($"Can`t delete, unlink all trainees from the direction");
            
            await _unitOfWork.Directions.Delete(direction);
            await _unitOfWork.Save();
            return _mapper.Map<DirectionDTO>(direction);
        }

        public async Task<DirectionDTO> Retrieve(int id)
        {
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == id, "Trainees");
            if (direction == null)
                throw new DataNotFoundExeption<Direction>("This direction doesn`t exist");
            return _mapper.Map<DirectionDTO>(direction);
        }

        public async Task Update(DirectionDTO directionDto)
        {
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == directionDto.Id, "Trainees");
            if (direction == null)
                throw new DataNotFoundExeption<Direction>("This direction doesn`t exist");
            if (await _unitOfWork.Directions.Retrieve(x => x.Name == directionDto.Name, "Trainees") != null)
                throw new DataDuplicateExeption<Direction>("Direction with this name exists");
            direction.Name = directionDto.Name;
            await _unitOfWork.Save();
        }

        public async Task<ServicesGetAllReturn<DirectionDTO>> GetAll(
            ServicesGetAllParameters<DirectionDTO> dataParams)
        {
            if (dataParams.Index < 0)
                throw new ArgumentOutOfRangeException("index < 0");

            Expression<Func<Direction, bool>>? predicate = null;
            if (dataParams.Name != null)
                predicate = d => d.Name == dataParams.Name;

            Func<IQueryable<Direction>, IOrderedQueryable<Direction>>? orderBy = null;
            switch (dataParams.SortKey)
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
                new ReposGetAllParameters<Direction>(
                    "Trainees", predicate, orderBy, dataParams.Descending, dataParams.Index, dataParams.Size));

            return new ServicesGetAllReturn<DirectionDTO>(
                directions.Entities.Select(x => _mapper.Map<DirectionDTO>(x)), directions.PageCount);
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            var directions = await _unitOfWork.Directions.GetAll(
                new ReposGetAllParameters<Direction>("Trainees", null, q => q.OrderBy(x => x.Id), false, 0, 100));
            return directions.Entities.Select(x => _mapper.Map<DirectionDTO>(x));
        }
    }
}
