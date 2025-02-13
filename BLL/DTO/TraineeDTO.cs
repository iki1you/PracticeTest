using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
namespace BLL.DTO
{
    public class TraineeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        [Key]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public DirectionDTO Direction { get; set; }
        public ProjectDTO Project { get; set; }
    }
}
