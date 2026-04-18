using Microsoft.Extensions.FileProviders;
using WebApi.Common.Middleware;

namespace WebApi.Extensions;

/// <summary>
/// Расширения для app builder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Работа с статичными файлами
    /// </summary>
    /// <param name="app"></param>
    /// <param name="environment"></param>
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
            RequestPath = "/api/files"
        });
        return app;
    }

    /// <summary>
    /// Добавляем middleware
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandler>();
        return app;
    }
}