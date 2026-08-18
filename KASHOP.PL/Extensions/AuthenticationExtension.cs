using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace KASHOP.PL;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Apisettings:Issuer"],
                ValidAudience = configuration["Apisettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Apisettings:SecretKey"]!)
                )
            };
        });
        
        services.AddAuthentication(); 

        return services;
    }
}
