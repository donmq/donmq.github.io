using System;
using API.Data;
using eTierV2_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eTierV2_API.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string factory = configuration.GetSection("AppSettings:Factory").Value;
            string area = configuration.GetSection("Appsettings:Area").Value;
            services.AddDbContext<DBContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_Connection")));
            // services.AddDbContext<CBDataContext>(options => options.UseSqlServer(configuration.GetConnectionString("CBConnection")));
            // services.AddDbContext<SPCDataContext>(options => options.UseSqlServer(configuration.GetConnectionString("SPCConnection")));
            // services.AddDbContext<TSHDataContext>(options => options.UseSqlServer(configuration.GetConnectionString("TSHConnection")));
            services.AddDbContext<MesDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_MESConnection")));
            services.AddDbContext<SHCQDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_QConnection")));
            services.AddDbContext<ciMESDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_ciMESConnection")));

        }
    }
}