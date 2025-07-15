using Machine_API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, bool isSAP)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var factory = configuration.GetSection("AppSettings:Factory").Value;
            var area = configuration.GetSection("AppSettings:Area").Value;

            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_DefaultConnection")).EnableSensitiveDataLogging());
            if (isSAP)
            {
                services.AddDbContext<MTContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_MTConnection")).EnableSensitiveDataLogging());
                services.AddDbContext<SAPContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_SAPConnection")).EnableSensitiveDataLogging());
            }
        }
    }
}