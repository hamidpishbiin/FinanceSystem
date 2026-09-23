using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Presentation
{
    public static class RegisterCORSExtensions
    {
        public static string CORSName = "_myAllowSpecificOrigins";

        public static WebApplicationBuilder AddCORS(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<Origins>()
                .Bind(builder.Configuration.GetSection(Origins.SectionName))
                .Validate(
                    o => !string.IsNullOrWhiteSpace(o.AllowOrigins),
                    $"{Origins.SectionName}:AllowOrigins must list at least one origin.")
                .ValidateOnStart();

            builder.Services.AddCors();

            builder.Services.AddOptions<CorsOptions>()
                .Configure<IOptions<Origins>>((cors, origins) =>
                {
                    var allowOrigins = origins.Value.AllowOrigins
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    cors.AddPolicy(name: CORSName,
                                   policy => policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowCredentials()
                                       .WithOrigins(allowOrigins)
                                       .SetIsOriginAllowed((host) => true));
                });

            return builder;
        }
    }
}
