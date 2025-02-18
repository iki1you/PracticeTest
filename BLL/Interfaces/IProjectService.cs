using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProjectService: ICRUDableService<ProjectDTO>
    {
        public IEnumerable<ProjectDTO> GetSortedByTrainees(IEnumerable<ProjectDTO> projectDTOs, bool descending);
        public IEnumerable<ProjectDTO> GetSortedByName(IEnumerable<ProjectDTO> projectDTOs, bool descending);
        public IEnumerable<ProjectDTO> GetRangeProjects(IEnumerable<ProjectDTO> projectDTOs, int index, int size);
        public IEnumerable<ProjectDTO> FindByName(IEnumerable<ProjectDTO> projects, string name);
    }
}
