using DotNetEnv;
using Persistence.Common;

namespace WebApi.Extensions;

public static class BuilderConfigurations
{
    public static IConfigurationBuilder AddEnvironment(this IConfigurationBuilder configBuilder, IConfiguration configuration)
    {
        var envPath = configuration.GetSection(EnvKeys.EnvironmentPath).Value;
        var p = Directory.GetCurrentDirectory();
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