using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace FinanceSystem.Security
{
    public static class RegisterAuthorizationConfig
    {
        public static WebApplicationBuilder AddAuthenticationByJwtToken(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JWTTokenConfig>(builder.Configuration.GetSection(JWTTokenConfig.SectionName));
            var secretKey = builder.Configuration[$"{JWTTokenConfig.SectionName}:SecretKey"];
            var issuer = builder.Configuration[$"{JWTTokenConfig.SectionName}:Issuer"];
            var audience = builder.Configuration[$"{JWTTokenConfig.SectionName}:Audience"];


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            return builder;
        }
    }
}
