using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service;

namespace Application.Util;

public static class ServiceConfig
{
    public static IServiceCollection AddRepositoryConfig(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<TokenRepository>();
        services.AddScoped<LocationRepository>();
        services.AddScoped<DeliveryRepository>();
        return services;
    }

    public static IServiceCollection AddServiceConfig(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<LocationService>();
        services.AddScoped<DeliveryService>();
        return services;
    }

    public static IServiceCollection AddAuthenticationConfig
    (this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtIssuer = configuration["Jwt:Issuer"] ?? string.Empty;
        var jwtAudience = configuration["Jwt:Audience"] ?? string.Empty;
        var jwtSecret = configuration["Jwt:Secret"] ?? string.Empty;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(jwtIssuer, jwtAudience, jwtSecret);
                options.Events = GetJwtBearerEvents();
            });

        return services;
    }

    private static TokenValidationParameters GetTokenValidationParameters(string jwtIssuer, string jwtAudience,
        string jwtSecret)
    {
        var encodedJwtSecret = Encoding.UTF8.GetBytes(jwtSecret);
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(encodedJwtSecret)
        };
    }

    private static JwtBearerEvents GetJwtBearerEvents() =>
        new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/location"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
}