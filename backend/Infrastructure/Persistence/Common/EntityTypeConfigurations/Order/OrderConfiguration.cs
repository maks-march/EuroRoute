using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderModel = Domain.Models.Order.Order;

namespace Persistence.Common.EntityTypeConfigurations.Order;

public class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
{
    public void Configure(EntityTypeBuilder<OrderModel> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);
        
        builder.Property(u => u.Status)
            .HasConversion<string>();
        
        // Связи с вложенными сущностями (один к одному)
        builder.HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Transport)
            .WithOne(p => p.Order)
            .HasForeignKey<Transport>(t => t.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связи с коллекциями (один ко многим)
        builder.HasMany(o => o.Payloads)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.RoutePoints)
            .WithOne(p => p.Order)
            .HasForeignKey(rp => rp.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
               
        // Настройка для массивов строк
        builder.HasMany(o => o.Photo)
            .WithOne(f => f.Owner)
            .HasForeignKey(f => f.OwnerId);
    }
}