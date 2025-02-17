using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

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
                throw new ArgumentNullException("direction doesn't exist");

            var project = _projectRepository.Retrieve(traineeDto.Direction.Id);
            if (project == null)
                throw new ArgumentNullException("project doesn't exist");

            if (!new PhoneAttribute().IsValid(traineeDto.Phone))
                throw new ValidationException("Wrong phone number");
            if (_traineeRepository.GetAll().Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Trainee with this phone exist's");

            if (!new EmailAddressAttribute().IsValid(traineeDto.Email))
                throw new ValidationException("Wrong email");
            if (_traineeRepository.Retrieve(traineeDto.Email) != null)
                throw new ArgumentException("Trainee with this email exist's");

            project.TraineeCount += 1;
            _traineeRepository.Create(new Trainee
            {
                Name = traineeDto.Name,
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
                throw new ArgumentNullException($"Trainee {traineeDto.Id} doesn't exist");
            var direction = _directionRepository.Retrieve(directionDto.Id);
            if (direction == null)
                throw new ArgumentException($"Direction {directionDto.Id} doesn't exist");

            trainee.Direction.TraineeCount -= 1;
            trainee.Direction = direction;
            direction.TraineeCount += 1;
        }

        public void AttachProject(TraineeDTO traineeDto, ProjectDTO projectDto)
        {
            var trainee = _traineeRepository.Retrieve(traineeDto.Id);
            if (trainee == null) 
                throw new ArgumentNullException($"Trainee {traineeDto.Id} doesn't exist");
            var project = _projectRepository.Retrieve(projectDto.Id);
            if (project == null)
                throw new ArgumentException($"Project {projectDto.Id} doesn't exist");

            trainee.Project.TraineeCount -= 1;
            trainee.Project = project;
            project.TraineeCount += 1;
        }

        public TraineeDTO Retrieve(int id)
        {
            var trainee = _traineeRepository.Retrieve(id);
            if (trainee == null)
                throw new ArgumentNullException("Trainee doesn`t exist");
            return new TraineeDTO
            {
                Id = trainee.Id,
                Name = trainee.Name,
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
                throw new ArgumentNullException("Trainee doesn`t exist");
            if (_traineeRepository.GetAll().Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Trainee with this phone exist's");
            if (_traineeRepository.GetAll().Any(x => x.Email == traineeDto.Email))
                throw new ArgumentException("Trainee with this email exist's");
            _traineeRepository.Update(new Trainee
            {
                Id = traineeDto.Id,
                Name = traineeDto.Name,
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
            var projectDto = Retrieve(id);

            var trainees = _traineeRepository.GetAll().Where(x => x.Project.Id == id);
            if (trainees.Any())
                throw new ArgumentException($"You can't delete, {trainees} subscribes to this project");

            _projectRepository.Delete(id);
            return projectDto;
        }

        public IEnumerable<TraineeDTO> GetAll()
        {
            return _traineeRepository.GetAll().Select(trainee => Retrieve(trainee.Id));
        }

        public IEnumerable<TraineeDTO> GetByProjectId(int Id)
        {
            return _traineeRepository.GetAll()
                .Where(trainee => trainee.Project.Id == Id)
                .Select(trainee => Retrieve(trainee.Id));
        }

        public IEnumerable<TraineeDTO> GetByDirectionId(int Id)
        {
            return _traineeRepository.GetAll()
                .Where(trainee => trainee.Direction.Id == Id)
                .Select(trainee => Retrieve(trainee.Id));
        }
    }
}
