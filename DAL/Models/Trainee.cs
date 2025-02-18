

using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Index(nameof(Phone), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Trainee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDay { get; set; }
        public Direction Direction { get; set; }
        public Project Project { get; set; }

    }
}
