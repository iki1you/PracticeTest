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
        // Нужно переделать
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations#collection-navigations
        public int TraineeCount { get; set; }
    }
}
