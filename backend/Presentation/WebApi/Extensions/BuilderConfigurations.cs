using DotNetEnv;
using Persistence;

namespace WebApi.Extensions;

public static class BuilderConfigurations
{
    public static IConfigurationBuilder AddEnvironment(this IConfigurationBuilder configBuilder, IConfiguration configuration)
    {
        var envPath = configuration.GetSection(EnvKeys.EnvironmentPath).Value;
        
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
        else
        {
            Console.WriteLine("Env file not found");
        }

        configBuilder.AddEnvironmentVariables();
        return configBuilder;
    }
}