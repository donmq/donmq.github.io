using AutoMapper;
using Machine_API.Helpers.AutoMapper;

namespace API.Configurations
{
    public static class AutoMapperRegisterConfig
    {
         public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IMapper>(sp => {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });

            services.AddSingleton(AutoMapperConfig.RegisterMappings());
        }
    }
}