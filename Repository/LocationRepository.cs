using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class LocationRepository(DelivereaseDbContext context)
{
    private readonly DbSet<Location> _locations = context.Set<Location>();

    public async Task AddAsync(Location location)
    {
        await _locations.AddAsync(location);
        await context.SaveChangesAsync();
    }
}