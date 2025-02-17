using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class StartupDAL
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<ITraineeRepository, TraineeRepository>();
            services.AddScoped<IDirectionRepository, DirectionRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
        }
    }
}
