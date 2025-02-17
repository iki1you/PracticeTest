using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class StartupBLL
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IDirectionService, DirectionService>();
            services.AddScoped<ITraineeService, TraineeService>();
            services.AddScoped<IProjectService, ProjectService>();
        }
    }
}
