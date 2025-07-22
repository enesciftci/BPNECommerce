using BPN.ECommerce.Domain.Aggregates.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPN.ECommerce.Infrastructure.Persistence.EFCore;

public class BpnDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
}