using Microsoft.Extensions.Logging;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.Samples.BasicClient;

/// <summary>
/// Sample console application demonstrating basic FreePBX client usage
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("FreePBX Client Sample");
        Console.WriteLine("======================\n");

        // Create client options
        var options = new FreepbxClientOptions("https://freepbx.example.com/graphql")
        {
            ApiKey = Environment.GetEnvironmentVariable("FREEPBX_API_KEY") ?? "your-api-key",
            TimeoutSeconds = 30,
            EnableRetry = true,
            MaxRetryAttempts = 3
        };

        // Create a simple logger
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        var logger = loggerFactory.CreateLogger<FreepbxClient>();

        // Create the client
        using var client = new FreepbxClient(options, logger);

        // Test connection
        Console.WriteLine("Testing connection...");
        var isConnected = await client.TestConnectionAsync();

        if (isConnected)
        {
            Console.WriteLine("✓ Connected successfully!\n");

            // Get all extensions
            Console.WriteLine("Fetching extensions...");
            var extensions = await client.Extensions.GetAllAsync();

            Console.WriteLine($"Found {extensions.Count} extensions:");
            foreach (var ext in extensions.Take(5))
            {
                Console.WriteLine($"  - {ext.Extension}: {ext.Name}");
            }

            // Get all queues
            Console.WriteLine("\nFetching queues...");
            var queues = await client.Queues.GetAllAsync();

            Console.WriteLine($"Found {queues.Count} queues:");
            foreach (var queue in queues.Take(5))
            {
                Console.WriteLine($"  - {queue.Extension}: {queue.Description}");
            }
        }
        else
        {
            Console.WriteLine("✗ Failed to connect to FreePBX");
        }

        Console.WriteLine("\nSample completed!");
    }
}