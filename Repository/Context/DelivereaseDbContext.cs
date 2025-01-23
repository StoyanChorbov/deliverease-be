using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository;

public class DelivereaseDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DelivereaseDbContext()
    {
    }

    public DelivereaseDbContext(DbContextOptions<DelivereaseDbContext> options) : base(options)
    {
    }
}