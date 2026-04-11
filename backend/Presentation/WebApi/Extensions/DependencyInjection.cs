using Application.Common.Mappings;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi;
using WebApi.DTO;

namespace WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        return services
            .AddConfiguredAutoMapper()
            .AddConfiguredControllers()
            .AddEndpointsApiExplorer()
            .AddConfiguredSwaggerGen();
    }

    private static IServiceCollection AddConfiguredSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token like: Bearer {token}"
            });
            c.AddSecurityRequirement(document =>
            {
                OpenApiSecuritySchemeReference? schemeRef = new("Bearer", document);
                OpenApiSecurityRequirement? requirement = new()
                {
                    [schemeRef] = []
                };
                return requirement;
            });
        });
        return services;
    }
    
    
    private static IServiceCollection AddConfiguredAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(
                new AssemblyMappingProfile(
                    typeof(DependencyInjection).Assembly,
                    typeof(Persistence.Extensions.DependencyInjection).Assembly,
                    typeof(Application.DependencyInjection).Assembly
                    )
                );
        });
        return services;
    }
    
    private static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            // Конвертируем названия controller и action
            options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseSlugParameterTransformer()));
        });
        return services;
    }

    private class LowercaseSlugParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value?.ToString()?.ToLowerInvariant();
        }
    }
}