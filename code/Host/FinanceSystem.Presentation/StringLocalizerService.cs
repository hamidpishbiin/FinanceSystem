using Microsoft.Extensions.Localization;
using FinanceSystem.Presentation.FinanceSystem.Presentation.Resources;
using Shared.Core;
using System.Reflection;

namespace FinanceSystem.Presentation
{
    public class StringLocalizerService : IStringLocalizerService
    {
        private readonly IStringLocalizer _localizer;

        public StringLocalizerService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public string GetString(string key)
        {
            return _localizer[key].Value;
        }
    }
}
