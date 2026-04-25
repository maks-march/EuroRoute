using Microsoft.OpenApi;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.DTO;

public class JsonDefaultFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema schemaOpenApi)
            return;
        foreach (var attribute in context.MemberInfo?.GetCustomAttributes<SwaggerJsonDefault>(false) ?? [])
        {
            schemaOpenApi.Default = attribute.ExampleJson;
        }
    }
}