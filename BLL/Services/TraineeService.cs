using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BLL.Services
{
    public class TraineeService: ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IDirectionRepository _directionRepository;
        private readonly IProjectRepository _projectRepository;

        public TraineeService(ITraineeRepository traineeRepository,
            IDirectionRepository directionRepository,
            IProjectRepository projectRepository) 
        {
            _traineeRepository = traineeRepository;
            _directionRepository = directionRepository;
            _projectRepository = projectRepository;
        }

        public void Create(TraineeDTO traineeDto)
        {
            var direction = _directionRepository.Retrieve(traineeDto.Direction.Id);
            if (direction == null)
                throw new ArgumentNullException("Направление не существует");

            var project = _projectRepository.Retrieve(traineeDto.Direction.Id);
            if (project == null)
                throw new ArgumentNullException("Проект не существует");

            if (!new PhoneAttribute().IsValid(traineeDto.Phone))
                throw new ValidationException("Неправильный формат номера телефона");
            if (traineeDto.Phone != null && _traineeRepository.GetAll().Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Стажер с таким номером телефона уже существует");

            if (!new EmailAddressAttribute().IsValid(traineeDto.Email))
                throw new ValidationException("Неправильный формат email");
            if (_traineeRepository.Retrieve(traineeDto.Email) != null)
                throw new ArgumentException("Стажер с таким email уже существует");

            project.TraineeCount += 1;
            direction.TraineeCount += 1;
            _traineeRepository.Create(new Trainee
            {
                Name = traineeDto.Name,
                Surname = traineeDto.Surname,
                Gender = traineeDto.Gender,
                Email = traineeDto.Email,
                Phone = traineeDto.Phone,
                BirthDay = traineeDto.BirthDay,
                Direction = direction,
                Project = project
            });
        }

        public void AttachDirection(TraineeDTO traineeDto, DirectionDTO directionDto)
        {
            var trainee = _traineeRepository.Retrieve(traineeDto.Id);
            if (trainee == null) 
                throw new ArgumentNullException($"Стажера {traineeDto.Id} не существует");
            var direction = _directionRepository.Retrieve(directionDto.Id);
            if (direction == null)
                throw new ArgumentException($"Стажера {directionDto.Id} не существует");

            trainee.Direction.TraineeCount -= 1;
            trainee.Direction = direction;
            direction.TraineeCount += 1;
        }

        public void AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto)
        {
            var trainee = _traineeRepository.Retrieve(traineeDto.Id);
            if (trainee == null) 
                throw new ArgumentNullException($"Стажера {traineeDto.Id} не существует");
            var project = _projectRepository.Retrieve(projectDto.Id);
            if (project == null)
                throw new ArgumentException($"Проекта {projectDto.Id} не существует");

            trainee.Project.TraineeCount -= 1;
            trainee.Project = project;
            project.TraineeCount += 1;
        }

        public TraineeDTO Retrieve(int id)
        {
            var trainee = _traineeRepository.Retrieve(id);
            if (trainee == null)
                throw new ArgumentNullException("Стажера не существует");
            return new TraineeDTO
            {
                Id = trainee.Id,
                Name = trainee.Name,
                Surname = trainee.Surname,
                Gender = trainee.Gender,
                Email = trainee.Email,
                Phone = trainee.Phone,
                BirthDay = trainee.BirthDay,
                Direction = new DirectionDTO
                {
                    Id = trainee.Direction.Id,
                    Name = trainee.Direction.Name,
                    TraineeCount = trainee.Direction.TraineeCount
                },
                Project = new ProjectDTO
                {
                    Id = trainee.Project.Id,
                    Name = trainee.Project.Name,
                    TraineeCount = trainee.Project.TraineeCount
                }
            };
        }

        public void Update(TraineeDTO traineeDto)
        {
            var trainee = _traineeRepository.Retrieve(traineeDto.Id);
            if (trainee == null)
                throw new ArgumentNullException("Стажер не существует");
            if (_traineeRepository.GetAll().Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Стажер с таким номером телефона уже существует");
            if (_traineeRepository.GetAll().Any(x => x.Email == traineeDto.Email))
                throw new ArgumentException("Стажер с таким email уже существует");
            _traineeRepository.Update(new Trainee
            {
                Id = traineeDto.Id,
                Name = traineeDto.Name,
                Surname = trainee.Surname,
                Gender = traineeDto.Gender,
                Email = traineeDto.Email,
                Phone = traineeDto.Phone,
                BirthDay = traineeDto.BirthDay,
                Direction = trainee.Direction,
                Project = trainee.Project
            });
        }

        public TraineeDTO Delete(int id)
        {
            var trainee = Retrieve(id);
            trainee.Project.TraineeCount -= 1;
            trainee.Direction.TraineeCount -= 1;
            _traineeRepository.Delete(id);
            return trainee;    
        }

        public IEnumerable<TraineeDTO> GetAll()
        {
            return _traineeRepository.GetAll().Select(trainee => Retrieve(trainee.Id));
        }

        public IEnumerable<IGrouping<ProjectDTO, TraineeDTO>> GroupByProjects(
            IEnumerable<TraineeDTO> traineesDto)
        {
            return traineesDto
                .Select(trainee => trainee)
                .GroupBy(x => x.Project);
        }

        public IEnumerable<IGrouping<DirectionDTO, TraineeDTO>> GroupByDirections(
            IEnumerable<TraineeDTO> traineesDto)
        {
            return traineesDto
                .Select(trainee => trainee)
                .GroupBy(x => x.Direction);
        }

        public IEnumerable<TraineeDTO> GetByDirectionId(int Id)
        {
            return _traineeRepository.GetAll()
                .Where(trainee => trainee.Direction.Id == Id)
                .Select(trainee => Retrieve(trainee.Id));
        }

        public IEnumerable<TraineeDTO> FilterByDirections(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<DirectionDTO> directions)
        {
            var ids = directions.Select(x => x.Id);
            return traineeDTOs.Where(x => ids.Contains(x.Direction.Id));
        }

        public IEnumerable<TraineeDTO> FilterByProjects(
            IEnumerable<TraineeDTO> traineeDTOs, IEnumerable<ProjectDTO> projects)
        {
            var ids = projects.Select(x => x.Id);
            return traineeDTOs.Where(x => ids.Contains(x.Project.Id));
        }
    }
}
