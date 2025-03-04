using AutoMapper;

namespace WebApi
{
    public static class ServiceConfigurer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            BLL.StartupBLL.Configure(services);
            DAL.StartupDAL.Configure(services);


            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
