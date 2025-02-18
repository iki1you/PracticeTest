using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDirectionService: ICRUDableService<DirectionDTO>
    {
        public IEnumerable<DirectionDTO> GetSortedByTrainees(IEnumerable<DirectionDTO> directionDTOs, bool descending);
        public IEnumerable<DirectionDTO> GetSortedByName(IEnumerable<DirectionDTO> directionDTOs, bool descending);
        public IEnumerable<DirectionDTO> GetRangeDirections(IEnumerable<DirectionDTO> directionDTOs, int index, int size);
        public IEnumerable<DirectionDTO> FindByName(IEnumerable<DirectionDTO> directions, string name);
    }
}
