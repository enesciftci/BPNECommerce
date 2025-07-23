using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPN.ECommerce.Domain.Aggregates.Orders.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasAlternateKey(o => o.OrderId);

        builder.Property(o => o.OrderId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Amount)
            .IsRequired();
        
        var converter = new ValueConverter<OrderStatus, string>(
            v => v.Value,
            v => new OrderStatus(v));

        builder.Property(o => o.Status)
            .HasConversion(converter)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(o => o.CompletedAt)
            .IsRequired(false);

        builder.Property(o => o.CancelledAt)
            .IsRequired(false);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired(false);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .HasPrincipalKey(o => o.OrderId);

    }
}
