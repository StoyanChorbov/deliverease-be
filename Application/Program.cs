using Application.Hubs;
using Application.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

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

        // Add database context
        builder.Services.AddDbContext<DelivereaseDbContext>(options => { options.UseNpgsql(connectionString); });

        // Add identity parameters
        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DelivereaseDbContext>()
            .AddDefaultTokenProviders();

        // Add repositories and services
        builder.Services
            .AddRepositoryConfig()
            .AddServiceConfig();

        // Add auth
        builder.Services.AddAuthenticationConfig(builder.Configuration);
        builder.Services.AddAuthorization();

        // Add real-time communication
        builder.Services.AddSignalR();

        // Register controller
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Limit to http only
        builder.WebHost.ConfigureKestrel(options =>
        {
            // options.ListenAnyIP(8080);
            options.ListenAnyIP(8081, listenOptions =>
            {
                // listenOptions.UseHttps("/https/aspnetcore.pfx", "");
            });
        });

        var app = builder.Build();

        // Add roles to database
        await SeedDatabase(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<LocationsHub>("/hubs/locations");

        app.MapControllers();

        app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        await app.RunAsync();
    }

    // Seed database with default roles
    private static async Task SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DelivereaseDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await context.Database.MigrateAsync();
        
        if (await context.Roles.AnyAsync())
            return;
        
        await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.User));
        await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
        await context.SaveChangesAsync();
    }
}