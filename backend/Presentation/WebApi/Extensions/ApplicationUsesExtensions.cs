using Microsoft.Extensions.FileProviders;
using Persistence;
using WebApi.Middleware;

namespace WebApi.Extensions;

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
    
    public static IApplicationBuilder UseStaticAssets(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        // Убеждаемся, что папка существует (важно при локальном запуске без докера)
        var uploadsPath = Path.Combine(environment.ContentRootPath, "wwwroot");
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        // Настраиваем раздачу статики
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/files"
        });
        return app;
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandler>();
        return app;
    }
}