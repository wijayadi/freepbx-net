using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx;

/// <summary>
/// Extension methods for registering FreePBX services in a dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers a <see cref="FreepbxClientOptions"/> singleton configured by the supplied delegate.
    /// </summary>
    public static IServiceCollection AddFreePbx(this IServiceCollection services, Action<FreepbxClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddSingleton(sp =>
        {
            var options = new FreepbxClientOptions();
            configure(options);
            return options;
        });

        return services.AddFreePbxCore();
    }

    /// <summary>
    /// Registers a <see cref="FreepbxClientOptions"/> singleton from the supplied instance.
    /// </summary>
    public static IServiceCollection AddFreePbx(this IServiceCollection services, FreepbxClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        services.AddSingleton(options);
        return services.AddFreePbxCore();
    }

    /// <summary>
    /// Binds <see cref="FreepbxClientOptions"/> from the specified configuration section
    /// (defaults to <c>FreePbx</c>) and registers the FreePBX services.
    /// </summary>
    public static IServiceCollection AddFreePbxFromConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "FreePbx")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var options = new FreepbxClientOptions();
        configuration.GetSection(sectionName).Bind(options);

        return services.AddFreePbx(options);
    }

    private static IServiceCollection AddFreePbxCore(this IServiceCollection services)
    {
        services.AddSingleton<IFreepbxClient>(sp =>
        {
            var options = sp.GetRequiredService<FreepbxClientOptions>();
            var loggerFactory = sp.GetService<ILoggerFactory>();
            return new FreepbxClient(options, loggerFactory);
        });

        services.AddSingleton<IExtensionService>(sp => sp.GetRequiredService<IFreepbxClient>().Extensions);
        services.AddSingleton<ICoreUserService>(sp => sp.GetRequiredService<IFreepbxClient>().CoreUsers);
        services.AddSingleton<ICoreDeviceService>(sp => sp.GetRequiredService<IFreepbxClient>().CoreDevices);
        services.AddSingleton<IRingGroupService>(sp => sp.GetRequiredService<IFreepbxClient>().RingGroups);
        services.AddSingleton<IInboundRouteService>(sp => sp.GetRequiredService<IFreepbxClient>().InboundRoutes);
        services.AddSingleton<IRecordingService>(sp => sp.GetRequiredService<IFreepbxClient>().Recordings);
        services.AddSingleton<IMusicOnHoldService>(sp => sp.GetRequiredService<IFreepbxClient>().MusicOnHold);
        services.AddSingleton<IVoiceMailService>(sp => sp.GetRequiredService<IFreepbxClient>().VoiceMail);
        services.AddSingleton<IFollowMeService>(sp => sp.GetRequiredService<IFreepbxClient>().FollowMe);
        services.AddSingleton<ICdrService>(sp => sp.GetRequiredService<IFreepbxClient>().Cdrs);
        services.AddSingleton<IQueueService>(sp => sp.GetRequiredService<IFreepbxClient>().Queues);

        return services;
    }
}
