namespace Machine_API.Helpers.Attributes
{
    public class DependencyInjectionAttribute : Attribute
    {
        public readonly ServiceLifetime ServiceLifetime;
        public readonly bool IsLoad = true;

        public DependencyInjectionAttribute(ServiceLifetime serviceLifetime, bool isCheckSAP = false)
        {
            ServiceLifetime = serviceLifetime;
            if (isCheckSAP)
            {
                var Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                var config = Configuration.GetValue<string>("AppSettings:IsSAP");
                IsLoad = bool.TryParse(config, out bool _isSAP) && _isSAP;
            }
        }
    }
}