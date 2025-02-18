using System.ComponentModel.DataAnnotations.Schema;


namespace DAL.Models
{
    [Microsoft.EntityFrameworkCore.Index("Email", IsUnique=true)]
    public class Trainee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDay { get; set; }
        [ForeignKey("DirectionId")]
        public Direction Direction { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

    }
}
