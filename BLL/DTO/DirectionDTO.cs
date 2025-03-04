using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class DirectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TraineeDTO> Trainees { get; } = new List<TraineeDTO>();
    }
}
