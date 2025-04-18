using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Repository;

public class DeliveryRepository(DelivereaseDbContext context)
{
    private readonly DbSet<Delivery> _deliveries = context.Set<Delivery>();

    public async Task AddAsync(Delivery delivery)
    {
        await _deliveries.AddAsync(delivery);
        await context.SaveChangesAsync();
    }

    public async Task<Delivery> GetAsync(Guid id)
    {
        var delivery = await _deliveries.FindAsync(id);

        if (delivery == null)
            throw new ArgumentException("Delivery not found");

        return delivery;
    }

    public async Task<List<Delivery>> GetAllAsync()
    {
        return await _deliveries.ToListAsync();
    }

    public async Task<List<Delivery>> GetAllByStartingAndEndingLocation(string startingLocationCity,
        string endingLocationCity)
    {
        var deliveries = await _deliveries
            .Where(d => d.StartingLocationRegion == startingLocationCity && d.EndingLocationRegion == endingLocationCity)
            .ToListAsync();

        return deliveries;
    }

    public async Task UpdateAsync(Delivery delivery)
    {
        var currentDelivery = await _deliveries.FindAsync(delivery.Id);

        if (currentDelivery == null)
            throw new ArgumentException("Delivery not found");

        _deliveries.Update(delivery);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var delivery = await _deliveries.FindAsync(id);

        if (delivery == null)
            throw new ArgumentException("Delivery not found");

        _deliveries.Remove(delivery);
        await context.SaveChangesAsync();
    }
}