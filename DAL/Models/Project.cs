namespace DAL.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Trainee> Trainees { get; } = new List<Trainee>();
    }
}
