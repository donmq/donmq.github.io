using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Configurations
{
    public static class DatabaseConfig
    {
         public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var area = configuration.GetSection("AppSettings:Area").Value;
            services.AddDbContext<DBContext>(options => options.UseSqlServer(configuration.GetConnectionString($"DefaultConnection_{area}")));
        }
    }
}