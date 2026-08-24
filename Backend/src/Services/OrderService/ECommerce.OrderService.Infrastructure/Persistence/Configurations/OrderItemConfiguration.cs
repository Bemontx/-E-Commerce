using ECommerce.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.OrderService.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(
        EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.OwnsOne(
            x => x.UnitPrice,
            money =>
            {
                money.Property(x => x.Amount)
                    .HasColumnName("unit_price")
                    .HasPrecision(18, 2);

                money.Property(x => x.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);
            });
    }
}