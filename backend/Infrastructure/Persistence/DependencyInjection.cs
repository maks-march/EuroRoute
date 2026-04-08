using System.Text;
using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Common.Auth;
using Persistence.Common.DbContexts;
using static Persistence.EnvKeys;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        
        var environment = configuration.GetSection(EnvironmentType).Value;
        var connectionString = configuration.GetConnectionString(DefaultConnection);
        
        return environment switch
        {
            Production => services.AddNpsqlContext(connectionString!),
            Development => services.AddSqliteContext(connectionString!),
            _ => throw new InvalidOperationException($"Unsupported environment: {environment}")
        };
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration[Issuer],
                    ValidAudience = configuration[Audience],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration[Secret]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();
        return services;
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
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
        return serviceProvider;
    }
    
    public static IServiceProvider ApplyMigrationsSqlite(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        // dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        return serviceProvider;
    }
}