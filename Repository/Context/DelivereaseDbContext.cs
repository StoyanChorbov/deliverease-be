using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository.Context;

public class DelivereaseDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DelivereaseDbContext()
    {
    }

    public DelivereaseDbContext(DbContextOptions<DelivereaseDbContext> options) : base(options)
    {
    }

    public DbSet<JwtToken> JwtTokens { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.Sender)
            .WithMany(u => u.SenderDeliveries)
            .HasForeignKey(d => d.SenderId);

        modelBuilder.Entity<Delivery>()
            .HasMany(d => d.Recipients)
            .WithMany(u => u.RecipientDeliveries)
            .UsingEntity(entityTypeBuilder => entityTypeBuilder.ToTable("DeliveryRecipients"));

        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.Deliverer)
            .WithMany(u => u.DelivererDeliveries)
            .HasForeignKey(d => d.DelivererId);

        modelBuilder.Entity<JwtToken>()
            .HasOne(t => t.User)
            .WithMany(u => u.JwtTokens)
            .HasForeignKey(t => t.UserId);

        base.OnModelCreating(modelBuilder);
    }
}