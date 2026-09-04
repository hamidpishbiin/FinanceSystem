namespace FinanceSystem.Presentation
{
    public static class RegisterCORSExtensions
    {
        public static string CORSName = "_myAllowSpecificOrigins";

        public static WebApplicationBuilder AddCORS(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<Origins>(builder.Configuration.GetSection(Origins.SectionName));
            var allowOrigins = builder.Configuration[$"{Origins.SectionName}:AllowOrigins"];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORSName,
                                  builder => builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                                      .WithOrigins(allowOrigins?.Split(",") ?? [])
                                      .SetIsOriginAllowed((host) => true));
            });
            return builder;
        }
    }
}
