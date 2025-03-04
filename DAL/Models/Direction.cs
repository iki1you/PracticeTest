using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Trainee> Trainees { get; } = new List<Trainee>();
    }
}
