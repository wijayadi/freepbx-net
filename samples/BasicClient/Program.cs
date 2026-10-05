using Microsoft.Extensions.Logging;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.Samples.BasicClient;

/// <summary>
/// Sample console application demonstrating basic FreePBX client usage.
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("FreePBX 17 Client Sample");
        Console.WriteLine("========================\n");

        var baseUrl = Environment.GetEnvironmentVariable("FREEPBX_BASE_URL") ?? "https://freepbx.example.com";

        var options = new FreepbxClientOptions(baseUrl)
        {
            ClientId = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_ID"),
            ClientSecret = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_SECRET"),
            Username = Environment.GetEnvironmentVariable("FREEPBX_USERNAME"),
            Password = Environment.GetEnvironmentVariable("FREEPBX_PASSWORD"),
            AccessToken = Environment.GetEnvironmentVariable("FREEPBX_ACCESS_TOKEN"),
            TimeoutSeconds = 30,
            AllowInsecureCertificates = true
        };

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        using var client = new FreepbxClient(options, loggerFactory);

        Console.WriteLine($"Testing connection to {baseUrl}...");
        var isConnected = await client.TestConnectionAsync();

        if (!isConnected)
        {
            Console.WriteLine("✗ Failed to connect to FreePBX. Check credentials and URL.");
            return;
        }

        Console.WriteLine("✓ Connected successfully!\n");

        var extensions = await client.Extensions.GetAllAsync();
        Console.WriteLine($"Found {extensions.Count} extensions:");
        foreach (var ext in extensions.Take(5))
        {
            Console.WriteLine($"  - {ext.ExtensionId}: {ext.Name} ({ext.Tech})");
        }

        var queues = await client.Queues.GetAllAsync();
        Console.WriteLine($"\nFound {queues.Count} queues:");
        foreach (var queue in queues.Take(5))
        {
            Console.WriteLine($"  - {queue.Extension}: {queue.Name}");
        }

        var ringGroups = await client.RingGroups.GetAllAsync();
        Console.WriteLine($"\nFound {ringGroups.Count} ring groups:");
        foreach (var group in ringGroups.Take(5))
        {
            Console.WriteLine($"  - {group.GroupNumber}: {group.Description}");
        }

        Console.WriteLine("\nSample completed!");
    }
}
