using DotNetEnv;
using Persistence.Common;

namespace WebApi.Extensions;

/// <summary>
/// Расширения для builder
/// </summary>
public static class BuilderConfigurations
{
    /// <summary>
    /// Подключение .env файла
    /// </summary>
    /// <param name="configBuilder"></param>
    /// <param name="configuration"></param>
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