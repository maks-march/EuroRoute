using Persistence.Extensions;

namespace WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        if (app.Environment.IsDevelopment())
        {
            scope.ServiceProvider.ApplyMigrationsSqlite();
        }
        if (app.Environment.IsProduction())
        {
            scope.ServiceProvider.ApplyMigrations();
        }

        return app;
    }

    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        await scope.ServiceProvider.CreateRoles();
        await scope.ServiceProvider.SeedAdminUserAsync(app.Configuration);
    }
}