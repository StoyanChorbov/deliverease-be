using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Repository.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DelivereaseDbContext>
{
    public DelivereaseDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<DelivereaseDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DelivereaseDbContext(optionsBuilder.Options);
    }
}