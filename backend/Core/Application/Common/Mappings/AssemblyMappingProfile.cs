using System.Reflection;
using AutoMapper;

namespace Application.Common.Mappings;

public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            ApplyMappingsFromAssembly(assembly);
        }
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo =  type.GetMethod("Mapping");
            methodInfo?.Invoke(instance, [this]);
        }
    }
}