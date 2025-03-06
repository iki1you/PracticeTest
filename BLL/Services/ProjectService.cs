using AutoMapper;
using BLL.DTO;
using BLL.Exeptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.FuncSignatures;
using System.Linq.Expressions;
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
                throw new DataDuplicateExeption<Project>("This project already exists");
            await _unitOfWork.Projects.Create(_mapper.Map<Project>(project));
            await _unitOfWork.Save();
        }

        public async Task<ProjectDTO> Delete(int id)
        {
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == id, "Trainees");

            if (project == null)
                throw new DataNotFoundExeption<Project>("This project doesn`t exist");
            if (project.Trainees.Count() > 0)
                throw new ArgumentException($"Can`t delete, unlink all trainees from the project");

            await _unitOfWork.Projects.Delete(project);
            await _unitOfWork.Save();
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<ProjectDTO> Retrieve(int id)
        {
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == id, "Trainees");
            if (project == null)
                throw new DataNotFoundExeption<Project>("This project doesn`t exist");
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task Update(ProjectDTO projectDto)
        {
            var project = await _unitOfWork.Projects.Retrieve(x => x.Id == projectDto.Id, "Trainees");
            if (project == null)
                throw new DataNotFoundExeption<Project>("This project doesn`t exist");
            if (await _unitOfWork.Projects.Retrieve(x => x.Name == projectDto.Name, "Trainees") != null)
                throw new DataDuplicateExeption<Project>("Project with this name exists");
            project.Name = projectDto.Name;
            await _unitOfWork.Save();
        }

        public async Task<(IEnumerable<ProjectDTO>, int)> GetAll(
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
                new GetAllParameters<Project>("Trainees", predicate, orderBy, descending, index, size));
            return (directions.Entities.Select(x => _mapper.Map<ProjectDTO>(x)), directions.PageCount);
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll()
        {
            var projects = await _unitOfWork.Projects.GetAll(
                new GetAllParameters<Project>("Trainees", null, q => q.OrderBy(x => x.Id), false, 0, 100));
            return projects.Entities.Select(x => _mapper.Map<ProjectDTO>(x));
        }
    }
}
