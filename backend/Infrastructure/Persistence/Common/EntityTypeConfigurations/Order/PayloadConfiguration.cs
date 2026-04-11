using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Common.EntityTypeConfigurations.Order;

public class PayloadConfiguration : OrderFieldConfiguration<Payload>
{
    public override void Configure(EntityTypeBuilder<Payload> builder)
    {
        builder.ToTable("Payloads");
        base.Configure(builder);
        builder.Property(u => u.Wrap)
            .HasConversion<string>();
    }
}