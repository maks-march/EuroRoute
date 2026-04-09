using Microsoft.Extensions.FileProviders;
using WebApi.Common.Middleware;

namespace WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
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

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandler>();
        return app;
    }
}