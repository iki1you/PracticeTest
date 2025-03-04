namespace BLL.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TraineeDTO> Trainees { get; } = new List<TraineeDTO>();
    }
}
