using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.DbContexts;
using static Persistance.Constants;

namespace Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = configuration.GetSection(Constants.Environment).Value;
        var connectionString = configuration.GetConnectionString(DefaultConnection);
        
        return environment switch
        {
            Production => services.AddNpsqlContext(connectionString!),
            Development => services.AddSqliteContext(connectionString!),
            _ => throw new InvalidOperationException($"Unsupported environment: {environment}")
        };
    }
    
    
    public static IServiceCollection AddNpsqlContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }
    
    public static IServiceCollection AddSqliteContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }
    
    public static IServiceProvider ApplyMigrations(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        return serviceProvider;
    }
    
    public static IServiceProvider ApplyMigrationsSqlite(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
        return serviceProvider;
    }
}