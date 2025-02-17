using DAL.EF;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProjectRepository: IProjectRepository, ICRUDableRepository<Project>
    {
        private ApplicationContext _db;

        public ProjectRepository(ApplicationContext context)
        {
            _db = context;
        }

        public Project? Retrieve(int id)
        {
            return _db.Projects.Find(id);
        }

        public void Create(Project project)
        {
            _db.Add(project);
        }

        public void Update(Project project)
        {
            _db.Update(project);
            _db.SaveChanges();
        }

        public Project? Delete(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
                return null;
            _db.Remove(id);
            _db.SaveChanges();
            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            return _db.Projects;
        }
    }
}
