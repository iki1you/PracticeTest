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

            if (!new EmailAddressAttribute().IsValid(traineeDto.Email))
                throw new ValidationException("Wrong email");
            if (_traineeRepository.Retrieve(traineeDto.Email) != null)
                throw new ArgumentException("Trainee with this email exist's");

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
    }
}
