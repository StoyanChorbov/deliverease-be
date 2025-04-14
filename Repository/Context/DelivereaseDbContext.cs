using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository.Context;

public class DelivereaseDbContext : IdentityDbContext<User>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }

    public DelivereaseDbContext()
    {
    }

    public DelivereaseDbContext(DbContextOptions<DelivereaseDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.SenderDeliveries)
            .WithOne(d => d.Sender)
            .HasForeignKey(d => d.DelivererId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.DelivererDeliveries)
            .WithOne(d => d.Deliverer)
            .HasForeignKey(d => d.DelivererId);
        
        base.OnModelCreating(modelBuilder);
    }
}