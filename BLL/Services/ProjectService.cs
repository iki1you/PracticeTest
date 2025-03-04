using AutoMapper;
using BLL.DTO;
using BLL.Exeptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.Globalization;
using System.Linq.Expressions;
using System.Xml.Linq;
using WebApi.Models;

namespace BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(ProjectDTO project)
        {
            var proj = await _unitOfWork.Projects.Retrieve(x => x.Name == project.Name, "Trainees");
            if (proj != null)
                throw new ProjectNotFoundException("This project already exists");
            await _unitOfWork.Projects.Create(_mapper.Map<Project>(project));
            await _unitOfWork.Save();
        }

        public async Task<ProjectDTO> Delete(int id)
        {
            var projectDto = await Retrieve(id);

            if (projectDto.Trainees.Count > 0)
                throw new ArgumentException($"Can`t delete, unlink all trainees from the project");

            await _unitOfWork.Projects.Delete(_mapper.Map<Project>(projectDto));
            await _unitOfWork.Save();
            return projectDto;
        }

        public async Task<ProjectDTO> Retrieve(int id)
        {
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == id, "Trainees");
            if (project == null)
                throw new ProjectNotFoundException("This project doesn`t exist");
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task Update(ProjectDTO projectDto)
        {
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == projectDto.Id, "Trainees");
            if (project == null)
                throw new ProjectNotFoundException("This project doesn`t exist");
            project.Name = projectDto.Name;
            await _unitOfWork.Projects.Update(project);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll(
            SortingKey sortKey, bool descending = false,
            int index = 0, int size = 10, string? name = null)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");

            Expression<Func<Project, bool>>? predicate = null;
            if (name != null)
                predicate = d => d.Name == name;

            Func<IQueryable<Project>, IOrderedQueryable<Project>>? orderBy = null;
            switch (sortKey)
            {
                case SortingKey.Name:
                    orderBy = q => q.OrderBy(d => d.Name);
                    break;
                case SortingKey.TraineeCount:
                    orderBy = q => q.OrderBy(d => d.Trainees.Count);
                    break;
            }

            var directions = await _unitOfWork.Projects.GetAll(
                "Trainees", predicate, orderBy, descending, index, size);
            return directions.Select(x => _mapper.Map<ProjectDTO>(x));
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll()
        {
            var projects = await _unitOfWork.Projects.GetAll("Trainees");
            return projects.Select(x => _mapper.Map<ProjectDTO>(x));
        }
    }
}
