using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Common.EntityTypeConfigurations.Order;

public class PaymentConfiguration : OrderFieldConfiguration<Payment>
{
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        base.Configure(builder);
        
        builder.Property(u => u.PaymentType)
            .HasConversion<string>();
    }
}