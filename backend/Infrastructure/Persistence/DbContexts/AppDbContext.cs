using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.EntityTypeConfigurations;

namespace Persistance.DbContexts;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<Order> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new TruckConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
        base.OnModelCreating(builder);
    }
}