using AutoMapper;
using DAL.Models;
using BLL.DTO;

namespace WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Trainee, TraineeDTO>();
            CreateMap<TraineeDTO, Trainee>();

            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();

            CreateMap<Direction, DirectionDTO>();
            CreateMap<DirectionDTO, Direction>();

            CreateMap<TraineeDTO, Dictionary<string, string>>()
                .ConvertUsing((trainee, notification) =>
                    new Dictionary<string, string>
                    {
                        { "id", trainee.Id.ToString() },
                        { "name", trainee.Name },
                        { "surname", trainee.Surname },
                        { "gender", trainee.Gender.ToString() },
                        { "email", trainee.Email },
                        { "phone", trainee.Phone ?? "" },
                        { "birthday", trainee.BirthDay.ToString("dd.MM.yyyy") },
                        { "project", trainee.Project.Name },
                        { "direction", trainee.Direction.Name }
                    });
        }
    }
}