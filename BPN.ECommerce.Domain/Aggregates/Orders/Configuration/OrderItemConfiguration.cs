namespace BPN.ECommerce.Domain.Aggregates.Orders.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        
        builder.Property(oi => oi.OrderId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(oi => oi.ProductId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.Price)
            .IsRequired();
    }
}
