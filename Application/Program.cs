using Microsoft.EntityFrameworkCore;
using Repository;
using Service;

namespace Application;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection")
            ?.Replace("{DATABASE}", Environment.GetEnvironmentVariable("DATABASE"))
            .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("{DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));

        // Add services to the container.
        builder.Services.AddDbContext<DelivereaseDbContext>(options => { options.UseNpgsql(connectionString); });

        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<UserService>();
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}