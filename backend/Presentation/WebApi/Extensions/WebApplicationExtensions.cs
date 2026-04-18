using Persistence.Extensions;

namespace WebApi.Extensions;

/// <summary>
/// Расширения для app
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Применяем миграции к бд
    /// </summary>
    /// <param name="app"></param>
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

    /// <summary>
    /// Стандартные данные в базе
    /// </summary>
    /// <param name="app"></param>
    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        await scope.ServiceProvider.CreateRoles();
        await scope.ServiceProvider.SeedAdminUserAsync(app.Configuration);
    }
}