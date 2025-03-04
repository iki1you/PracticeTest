using AutoMapper;
using BLL.DTO;
using BLL.Exeptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using WebApi.Models;

namespace BLL.Services
{
    public class TraineeService: ITraineeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TraineeService(IMapper mapper, IUnitOfWork unitOfWork) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(TraineeDTO traineeDto)
        {
            var direction = _unitOfWork.Directions.Retrieve(x => x.Id == traineeDto.Direction.Id);
            if (direction == null)
                throw new DirectionNotFoundException("Direction doesn`t exist");

            var project = _unitOfWork.Projects.Retrieve(x => x.Id == traineeDto.Project.Id);
            if (project == null)
                throw new ProjectNotFoundException("Project doesn`t exist");

            if (!new PhoneAttribute().IsValid(traineeDto.Phone))
                throw new ValidationException("Wrong phone number format");

            // Обязательно вынести проверку дубликата в бд.
            // Очень затратно загружать всю таблицу целиком, тем более синхронно, после чего перебирать все элементы без индекса.
            // При большом размере таблиы текущее решение упадет, съест всю оперативку и будет тормозить вообще весь сервер.
            if ((traineeDto.Phone != null) &&
                (_unitOfWork.Trainees.Retrieve(x => x.Phone == traineeDto.Phone).Result != null) )
                throw new ArgumentException("Trainee with this phone number already exists");

            if (!new EmailAddressAttribute().IsValid(traineeDto.Email))
                throw new ValidationException("Wrong email format");

            if (_unitOfWork.Trainees.Retrieve(x => x.Email == traineeDto.Email) != null)
                throw new ArgumentException("Trainee with this email already exists");

            await _unitOfWork.Trainees.Create(_mapper.Map<Trainee>(traineeDto));
            await _unitOfWork.Save();
        }

        public async Task AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto)
        {
            var trainee = await _unitOfWork.Trainees.Retrieve(x => x.Id == traineeDto.Id);
            if (trainee == null) 
                throw new ArgumentNullException($"Trainee {traineeDto.Id} doesn`t exist");
            var direction = await _unitOfWork.Directions.Retrieve(x => x.Id == directionDto.Id);
            if (direction == null)
                throw new DirectionNotFoundException($"Direction {directionDto.Id} doesn`t exist");
            trainee.Direction = direction;
            await _unitOfWork.Trainees.Update(trainee);
            await _unitOfWork.Directions.Update(direction);
            await _unitOfWork.Save();
        }

        public async Task AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto)
        {
            var trainee = await _unitOfWork.Trainees.Retrieve(x => x.Id == traineeDto.Id);
            if (trainee == null) 
                throw new ArgumentNullException($"Trainee {traineeDto.Id} doesn`t exist");
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == projectDto.Id);
            if (project == null)
                throw new ProjectNotFoundException($"Project {projectDto.Id} doesn`t exist");
            trainee.Project = project;
            await _unitOfWork.Trainees.Update(trainee);
            await _unitOfWork.Projects.Update(project);
            await _unitOfWork.Save();
        }

        public async Task Update(TraineeDTO traineeDto)
        {
            var trainee = await _unitOfWork.Trainees.Retrieve(x => x.Id == traineeDto.Id);
            var trainees = await _unitOfWork.Trainees.GetAll("", x => x.Id != traineeDto.Id);
            // Валидацию можно вынести в отдельный класс, это удобно сделать с FluentValidation
            if (trainee == null)
                throw new NullReferenceException("Trainee doesn`t exist");
            if (traineeDto.Phone != null && trainees.Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Trainee with this phone number already exists");
            if (trainees.Any(x => x.Email == traineeDto.Email))
                throw new ArgumentException("Trainee with this email number already exists");

            trainee.Name = traineeDto.Name;
            trainee.Surname = traineeDto.Surname;
            trainee.Gender = (Gender)traineeDto.Gender;
            trainee.Email = traineeDto.Email;
            trainee.BirthDay = traineeDto.BirthDay;
            trainee.Phone = traineeDto.Phone;
            await _unitOfWork.Trainees.Update(trainee);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<TraineeDTO>> GetAll(
            SortingKey sortKey, bool descending = false,
            int index = 0, int size = 10, string? name = null)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");

            Expression<Func<Trainee, bool>>? predicate = null;
            if (name != null)
                predicate = d => d.Name == name;


            var directions = await _unitOfWork.Trainees.GetAll(
                "", predicate, orderBy: null, descending, index, size);
            return directions.Select(x => _mapper.Map<TraineeDTO>(x));
        }

        public async Task<TraineeDTO> Retrieve(int id)
        {
            var trainee = await _unitOfWork.Trainees.Retrieve(x => x.Id == id, "Direction,Project");
            if (trainee == null)
                throw new NullReferenceException("Trainee not found");
            return _mapper.Map<TraineeDTO>(trainee);
        }

        public async Task<TraineeDTO> Retrieve(string email)
        {
            var trainee = await _unitOfWork.Trainees.Retrieve(x => x.Email == email, "Direction,Project");
            if (trainee == null)
                throw new NullReferenceException("Trainee not found");
            return _mapper.Map<TraineeDTO>(trainee);
        }

        public IEnumerable<TraineeDTO> FilterByDirections(
            IEnumerable<TraineeDTO> traineeDTOs, int directionId)
        {
            return traineeDTOs.Where(x => x.Direction.Id == directionId);
        }

        public IEnumerable<TraineeDTO> FilterByProjects(
            IEnumerable<TraineeDTO> traineeDTOs, int projectId)
        {
            return traineeDTOs.Where(x => x.Project.Id == projectId);
        }

        public Task<TraineeDTO> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TraineeDTO>> GetAll()
        {
            var trainees = await _unitOfWork.Trainees.GetAll(
                "Project,Direction", null, q => q.OrderBy(x => x.Id), false, 0, 100);
            return trainees.Select(x => _mapper.Map<TraineeDTO>(x));
        }
    }
}
