using BLL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProjectService
    {
        public void Create(ProjectDTO projectDto);
        
        public ProjectDTO Retrieve(Guid id);
        public void Update(ProjectDTO projectDto);
        public ProjectDTO Delete(Guid id);
        public IEnumerable<Project> GetAll();
    }
}
