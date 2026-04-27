using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;

namespace Sengsara.Freepbx.Samples.WorkerService;

/// <summary>
/// Background worker that periodically syncs FreePBX data
/// </summary>
public class FreePbxSyncWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FreePbxSyncWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public FreePbxSyncWorker(
        IServiceProvider serviceProvider,
        ILogger<FreePbxSyncWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FreePBX Sync Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SyncDataAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sync");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("FreePBX Sync Worker stopped");
    }

    private async Task SyncDataAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting data sync...");

        using var scope = _serviceProvider.CreateScope();
        var extensionService = scope.ServiceProvider.GetRequiredService<IExtensionService>();
        var queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();

        // Sync extensions
        var extensions = await extensionService.GetAllAsync(cancellationToken);
        _logger.LogInformation("Synced {Count} extensions", extensions.Count);

        // Sync queues
        var queues = await queueService.GetAllAsync(cancellationToken);
        _logger.LogInformation("Synced {Count} queues", queues.Count);

        // Sync queue members
        foreach (var queue in queues)
        {
            var members = await queueService.GetMembersAsync(queue.Id, cancellationToken);
            _logger.LogDebug("Queue {Extension} has {Count} members", queue.Extension, members.Count);
        }

        _logger.LogInformation("Data sync completed");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Add FreePBX services
                services.AddFreePbx(options =>
                {
                    options.Endpoint = hostContext.Configuration["Freepbx:Endpoint"] ?? "http://localhost:4000/graphql";
                    options.ApiKey = hostContext.Configuration["Freepbx:ApiKey"];
                    options.TimeoutSeconds = 30;
                });

                // Add the background worker
                services.AddHostedService<FreePbxSyncWorker>();
            })
            .Build()
            .Run();
    }
}