using Application.Common.Mappings;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguredAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(
                new AssemblyMappingProfile(
                    typeof(WebApi.DependencyInjection).Assembly,
                    typeof(Persistence.DependencyInjection).Assembly,
                    typeof(Application.DependencyInjection).Assembly
                    )
                );
        });
        return services;
    }
    
    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            // Конвертируем названия controller и action
            options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseSlugParameterTransformer()));
        });
        return services;
    }

    public class LowercaseSlugParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value?.ToString()?.ToLowerInvariant();
        }
    }
}