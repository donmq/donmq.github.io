using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Configurations
{
    public static class DatabaseConfig
    {
         public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var factory = configuration.GetSection("AppSettings:Factory").Value;
            var area = configuration.GetSection("AppSettings:Area").Value;
            var connection = $"{factory}_{area}_DefaultConnection";
            services.AddDbContext<DBContext>(options => options.UseSqlServer(configuration.GetConnectionString(connection)));
        }
    }
}