using System.Reflection;
using Microsoft.Extensions.Localization;

namespace Machine_API.Resources
{
    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;
        public LocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(Resource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
            _localizer = factory.Create("Resource", assemblyName.Name);
        }

        public string GetLocalizedHtmlString(string key)
        {
            return _localizer[key].Value;
        }
    }
}