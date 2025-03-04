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
        }
    }
}