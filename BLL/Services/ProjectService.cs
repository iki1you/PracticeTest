﻿using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(ITraineeRepository traineeRepository,
            IDirectionRepository directionRepository,
            IProjectRepository projectRepository)
        {
            _traineeRepository = traineeRepository;
            _projectRepository = projectRepository;
        }

        public void Create(ProjectDTO project)
        {
            if (_projectRepository.GetAll().Any( x => x.Name == project.Name ))
                throw new ArgumentException("Проект с таким именем существует");
            _projectRepository.Create(new Project {
                Name = project.Name
            });
        }

        public ProjectDTO Delete(int id)
        {
            var projectDto = Retrieve(id);

            var trainees = _traineeRepository.GetAll().Where(x => x.Project.Id == id);
            if (trainees.Any())
                throw new ArgumentException($"Нельзя удалить, {trainees} подписаны на этот проект");

            _projectRepository.Delete(id);
            return projectDto;
        }

        public ProjectDTO Retrieve(int id)
        {
            var project = _projectRepository.Retrieve(id);
            if (project == null)
                throw new ArgumentNullException("Такого проекта не существует");
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                TraineeCount = project.TraineeCount
            };
        }

        public void Update(ProjectDTO projectDto)
        {
            var project = _projectRepository.Retrieve(projectDto.Id);
            if (project == null)
                throw new ArgumentNullException("Такого проекта не существует");
            _projectRepository.Update(new Project
            {
                Id = projectDto.Id,
                Name = projectDto.Name,
                TraineeCount = project.TraineeCount
            });
        }

        public IEnumerable<ProjectDTO> GetAll()
        {
            return _projectRepository.GetAll()
                .Select(x => new ProjectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    TraineeCount = x.TraineeCount
                });
        }

        public IEnumerable<ProjectDTO> GetSortedByTrainees(IEnumerable<ProjectDTO> projectDTOs, bool descending)
        {
            var sorted = projectDTOs.OrderBy(x => x.TraineeCount);
            return descending ? sorted.Reverse() : sorted;
        }

        public IEnumerable<ProjectDTO> GetSortedByName(IEnumerable<ProjectDTO> projectDTOs, bool descending)
        {
            var sorted = projectDTOs.OrderBy(x => x.Name);
            return descending ? sorted.Reverse() : sorted;
        }

        public IEnumerable<ProjectDTO> GetRangeProjects(IEnumerable<ProjectDTO> projectDTOs, int index, int size)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index < 0");
            return projectDTOs.Skip(index * size).Take(size);
        }

        public IEnumerable<ProjectDTO> FindByName(IEnumerable<ProjectDTO> projects, string name)
        {
            return projects
                .Where(project => project.Name.Contains(name))
                .Select(project => Retrieve(project.Id));
        }
    }
}
