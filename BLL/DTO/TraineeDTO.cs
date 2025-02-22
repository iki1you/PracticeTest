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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public GenderDTO Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [RegularExpression(@"\+7\d{10}", ErrorMessage = "Невалидный номер телефона")]
        public string? Phone { get; set; }
        [Required]
        public DateOnly BirthDay { get; set; }
        [Required]
        public DirectionDTO Direction { get; set; }
        [Required]
        public ProjectDTO Project { get; set; }
    }
}
