using Domain.Models;
using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using File = Domain.Models.File;

namespace Application.Interfaces;

public interface IAppDbContext
{
    public DbSet<User> BusinessUsers { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payload> Payloads { get; set; }
    public DbSet<RoutePoint> RoutePoints { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<File> Files { get; set; }

    public ChangeTracker ChangeTracker { get; }
    public DbSet<T> Set<T>() where T : OrderCollectionField;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}