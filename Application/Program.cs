using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository;
using Repository.Context;
using Service;

namespace Application;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection")
            ?.Replace("${DB_CONN}", Environment.GetEnvironmentVariable("DB_CONN"))
            .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));

        // Add services to the container.
        builder.Services.AddDbContext<DelivereaseDbContext>(options => { options.UseNpgsql(connectionString); });

        builder.Services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DelivereaseDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<TokenRepository>();
        builder.Services.AddScoped<UserService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? string.Empty))
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.WebHost.ConfigureKestrel(options =>
        {
            // options.ListenAnyIP(8080);
            options.ListenAnyIP(8081, listenOptions =>
            {
                // listenOptions.UseHttps("/https/aspnetcore.pfx", "");
            });
        });

        var app = builder.Build();

        await SeedDatabase(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        await app.RunAsync();
    }

    private static async Task SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DelivereaseDbContext>();
        await context.Database.MigrateAsync();
        await context.Roles.AddAsync(new IdentityRole<Guid>(UserRoles.User));
        await context.Roles.AddAsync(new IdentityRole<Guid>(UserRoles.Admin));
    }
}