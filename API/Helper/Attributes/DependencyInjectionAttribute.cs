namespace API.Helper.Attributes
{
    public class DependencyInjectionAttribute : Attribute
    {
        public readonly ServiceLifetime ServiceLifetime;

        public DependencyInjectionAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}