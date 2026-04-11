using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Common.EntityTypeConfigurations.Order;

public class RoutePointsConfiguration : OrderFieldConfiguration<RoutePoints>
{
    public override void Configure(EntityTypeBuilder<RoutePoints> builder)
    {
        builder.ToTable("RoutePoints");
        base.Configure(builder);
        builder.Property(rp => rp.LoadTimeStart)
            .HasColumnType("interval");
        builder.Property(rp => rp.LoadTimeEnd)
            .HasColumnType("interval");
    }
}