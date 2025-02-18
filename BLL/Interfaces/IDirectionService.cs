using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDirectionService
    {
        public void Create(DirectionDTO directionDto);

        public DirectionDTO Retrieve(Guid id);
        public void Update(DirectionDTO directionDto);
        public DirectionDTO Delete(Guid id);
    }
}
