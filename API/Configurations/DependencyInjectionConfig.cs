using System.Reflection;
using API.Helper.Attributes;

namespace API.Configurations
{
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Add the services which has been registered with annotation to the DI
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            var implementedTypes = GetTypesWith<DependencyInjectionAttribute>(true);

            foreach (var implementedType in implementedTypes)
            {
                var types = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Contains(implementedType));

                foreach (var type in types)
                {
                    var attribute = implementedType.GetCustomAttribute<DependencyInjectionAttribute>();
                    if (attribute != null)
                        services.Add(new ServiceDescriptor(implementedType, type, attribute.ServiceLifetime));
                }
            }

            return services;
        }

        private static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }
    }
}