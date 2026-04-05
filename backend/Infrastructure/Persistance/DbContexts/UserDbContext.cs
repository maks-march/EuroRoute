using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.EntityTypeConfigurations;

namespace Persistance.DbContexts;

public class UserDbContext : DbContext, IUserDbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(builder);
    }
}