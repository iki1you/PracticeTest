using DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace DAL.EF
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Direction> Directions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
        }
    }
}
