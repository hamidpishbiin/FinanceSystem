using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace FinanceSystem.Security
{
    public static class RegisterAuthorizationConfig
    {
        private const int MinimumSecretKeyBytes = 32;

        public static WebApplicationBuilder AddAuthenticationByJwtToken(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<JWTTokenConfig>()
                .Bind(builder.Configuration.GetSection(JWTTokenConfig.SectionName))
                .Validate(
                    c => !string.IsNullOrWhiteSpace(c.SecretKey) && Encoding.UTF8.GetByteCount(c.SecretKey) >= MinimumSecretKeyBytes,
                    $"{JWTTokenConfig.SectionName}:SecretKey must be at least {MinimumSecretKeyBytes} bytes for HMAC-SHA256.")
                .Validate(
                    c => !string.IsNullOrWhiteSpace(c.Issuer),
                    $"{JWTTokenConfig.SectionName}:Issuer is required.")
                .Validate(
                    c => !string.IsNullOrWhiteSpace(c.Audience),
                    $"{JWTTokenConfig.SectionName}:Audience is required.")
                .ValidateOnStart();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

            builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JWTTokenConfig>>((jwtBearer, tokenConfig) =>
                {
                    var config = tokenConfig.Value;

                    jwtBearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey))
                    };
                });

            return builder;
        }
    }
}
