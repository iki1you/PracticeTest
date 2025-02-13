using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
