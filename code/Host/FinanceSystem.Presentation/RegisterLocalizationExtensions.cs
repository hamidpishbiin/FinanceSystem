using Microsoft.AspNetCore.Localization;
using Shared.Core;
using System.Globalization;

namespace FinanceSystem.Presentation
{
    public static class RegisterLocalizationExtensions
    {
        public static IServiceCollection RegisterLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<IStringLocalizerService, StringLocalizerService>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("es-ES")
                };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = [
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()];
            });
            return services;
        }
    }
}
