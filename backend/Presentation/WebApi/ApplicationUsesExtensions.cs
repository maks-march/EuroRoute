using Microsoft.Extensions.FileProviders;
using Persistance;

namespace WebApi;

public static class ApplicationUsesExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            if (app.Environment.IsDevelopment())
            {
                scope.ServiceProvider.ApplyMigrationsSqlite();
            }
            if (app.Environment.IsProduction())
            {
                scope.ServiceProvider.ApplyMigrations();
            }
        }
    }
    
    public static void UseStaticAssets(this IApplicationBuilder host, IWebHostEnvironment environment)
    {
        // Убеждаемся, что папка существует (важно при локальном запуске без докера)
        var uploadsPath = Path.Combine(environment.ContentRootPath, "wwwroot");
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        // Настраиваем раздачу статики
        host.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/files"
        });
    }
}