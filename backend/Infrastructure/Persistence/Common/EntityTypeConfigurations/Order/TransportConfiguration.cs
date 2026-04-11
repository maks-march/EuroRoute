using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Common.EntityTypeConfigurations.Order;

public class TransportConfiguration : OrderFieldConfiguration<Transport>
{
    public override void Configure(EntityTypeBuilder<Transport> builder)
    {
        builder.ToTable("Transports");
        base.Configure(builder);
        
        builder
            .Property(t => t.BodyType)
            .HasColumnType("text[]");
        builder
            .Property(t => t.LoadType)
            .HasColumnType("text[]");
        builder
            .Property(t => t.UnloadType)
            .HasColumnType("text[]");
    }
}