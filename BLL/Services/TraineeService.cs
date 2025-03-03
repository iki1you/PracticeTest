using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;

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
                // Текст в исключении лучше писать на английском. Исключение ArgumentNullException путает, у метода нет такого параметра.
                // Здесь лучше создать свой тип исключения.
                throw new ArgumentNullException("Направление не существует");

            var project = _projectRepository.Retrieve(traineeDto.Project.Id);
            if (project == null)
                throw new ArgumentNullException("Проект не существует");

            if (!new PhoneAttribute().IsValid(traineeDto.Phone))
                throw new ValidationException("Неправильный формат номера телефона");
            // Обязательно вынести проверку дубликата в бд.
            // Очень затратно загружать всю таблицу целиком, тем более синхронно, после чего перебирать все элементы без индекса.
            // При большом размере таблиы текущее решение упадет, съест всю оперативку и будет тормозить вообще весь сервер.
            if (traineeDto.Phone != null && _traineeRepository.GetAll().Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Стажер с таким номером телефона уже существует");

            if (!new EmailAddressAttribute().IsValid(traineeDto.Email))
                throw new ValidationException("Неправильный формат email");

            if (_traineeRepository.Retrieve(traineeDto.Email) != null)
                throw new ArgumentException("Стажер с таким email уже существует");
            // При вызове метода Retrieve и сохранении данных в Create неочевидно использование ChangeTracker-а.
            // Нужно использовать CollectionNavigation https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations#collection-navigations
            // и не обновлять значения TraineeCount вручную. Это замечание обязательно исправить.
            project.TraineeCount += 1;
            direction.TraineeCount += 1;
            // Здесь лучше использовать маппер, зачем нужен GenderDTO?
            _traineeRepository.Create(new Trainee
            {
                Name = traineeDto.Name,
                Surname = traineeDto.Surname,
                Gender = (Gender)traineeDto.Gender,
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
            _traineeRepository.Update(trainee);
            _directionRepository.Update(direction);
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
            _traineeRepository.Update(trainee);
            _projectRepository.Update(project);
        }

        public void Update(TraineeDTO traineeDto)
        {
            var trainee = _traineeRepository.Retrieve(traineeDto.Id);
            var trainees = _traineeRepository.GetAll().Where(x => x.Id != traineeDto.Id);
            // Валидацию можно вынести в отдельный класс, это удобно сделать с FluentValidation
            if (trainee == null)
                throw new ArgumentNullException("Стажер не существует");
            if (traineeDto.Phone != null && trainees.Any(x => x.Phone == traineeDto.Phone))
                throw new ArgumentException("Стажер с таким номером телефона уже существует");
            if (trainees.Any(x => x.Email == traineeDto.Email))
                throw new ArgumentException("Стажер с таким email уже существует");

            trainee.Name = traineeDto.Name;
            trainee.Surname = traineeDto.Surname;
            trainee.Gender = (Gender)traineeDto.Gender;
            trainee.Email = traineeDto.Email;
            trainee.BirthDay = traineeDto.BirthDay;
            trainee.Phone = traineeDto.Phone;
            _traineeRepository.Update(trainee);
        }

        public IEnumerable<TraineeDTO> GetAll() => 
            _traineeRepository.GetAll()
            .Select(trainee => new TraineeDTO
            {
                Id = trainee.Id,
                Name = trainee.Name,
                Surname = trainee.Surname,
                Gender = (GenderDTO)trainee.Gender,
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
            }).OrderBy(x => x.Id);
        
        public IEnumerable<(ProjectDTO, IEnumerable<TraineeDTO>)> GroupByProjects(
            IEnumerable<ProjectDTO> projectsDto,
            IEnumerable<TraineeDTO> traineesDto,
            SortingKey sortKey, bool descending) => projectsDto
                .Select(project => (project, traineesDto.Where(trainee => trainee.Project.Id == project.Id)));

        public IEnumerable<(DirectionDTO, IEnumerable<TraineeDTO>)> GroupByDirections(
            IEnumerable<DirectionDTO> directionsDto, 
            IEnumerable<TraineeDTO> traineesDto, 
            SortingKey sortKey, bool descending) => directionsDto
                .Select(direction => (direction, traineesDto.Where(trainee => trainee.Direction.Id == direction.Id)));

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

        public TraineeDTO Retrieve(IEnumerable<TraineeDTO> items, int id)
        {
            var trainee = items.First(x => x.Id == id);
            if (trainee == null)
                throw new NullReferenceException("Стажер не найден");
            return trainee;
        }

        public TraineeDTO Retrieve(IEnumerable<TraineeDTO> items, string email)
        {
            var trainee = items.First(x => x.Email == email);
            if (trainee == null)
                throw new NullReferenceException("Стажер не найден");
            return trainee;
        }
    }
}
