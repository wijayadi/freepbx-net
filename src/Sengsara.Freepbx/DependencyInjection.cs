using System;
using Microsoft.Extensions.DependencyInjection;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Client;
using Sengsara.Freepbx.Services;

namespace Sengsara.Freepbx;

/// <summary>
/// Extension methods for registering FreePBX services in the DI container
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds FreePBX services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configure">Configuration options</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddFreePbx(
        this IServiceCollection services,
        Action<Abstractions.Interfaces.FreepbxClientOptions> configure)
    {
        var options = new Abstractions.Interfaces.FreepbxClientOptions();
        configure(options);

        services.AddSingleton<IFreepbxClient>(sp =>
        {
            var clientOptions = new Client.FreepbxClientOptions
            {
                Endpoint = options.Endpoint,
                ApiKey = options.ApiKey,
                Username = options.Username,
                Password = options.Password,
                TimeoutSeconds = options.TimeoutSeconds,
                EnableRetry = options.EnableRetry,
                MaxRetryAttempts = options.MaxRetryAttempts
            };
            var logger = sp.GetService<Microsoft.Extensions.Logging.ILogger<Client.FreepbxClient>>();
            return new Client.FreepbxClient(clientOptions, logger);
        });

        services.AddSingleton<IExtensionService, ExtensionService>();
        services.AddSingleton<IQueueService, QueueService>();

        return services;
    }

    /// <summary>
    /// Adds FreePBX services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="options">Client options</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddFreePbx(
        this IServiceCollection services,
        Abstractions.Interfaces.FreepbxClientOptions options)
    {
        services.AddSingleton<IFreepbxClient>(sp =>
        {
            var clientOptions = new Client.FreepbxClientOptions
            {
                Endpoint = options.Endpoint,
                ApiKey = options.ApiKey,
                Username = options.Username,
                Password = options.Password,
                TimeoutSeconds = options.TimeoutSeconds,
                EnableRetry = options.EnableRetry,
                MaxRetryAttempts = options.MaxRetryAttempts
            };
            var logger = sp.GetService<Microsoft.Extensions.Logging.ILogger<Client.FreepbxClient>>();
            return new Client.FreepbxClient(clientOptions, logger);
        });

        services.AddSingleton<IExtensionService, ExtensionService>();
        services.AddSingleton<IQueueService, QueueService>();

        return services;
    }
}