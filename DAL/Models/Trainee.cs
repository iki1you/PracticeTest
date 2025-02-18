using System.ComponentModel.DataAnnotations;


namespace DAL.Models
{
    public class Trainee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        [Key]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public Direction Direction { get; set; }
        public Project Project { get; set; }

    }
}
