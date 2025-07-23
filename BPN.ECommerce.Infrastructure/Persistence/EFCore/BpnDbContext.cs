using BPN.ECommerce.Domain.Aggregates.Orders.Configuration;
using BPN.ECommerce.Domain.Aggregates.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPN.ECommerce.Infrastructure.Persistence.EFCore;

public class BpnDbContext : DbContext
{
    public BpnDbContext(DbContextOptions<BpnDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BpnDbContext).Assembly,
            type => type != typeof(OrderConfiguration));
    }
}