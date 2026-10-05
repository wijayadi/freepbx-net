using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.Tools.SchemaFetcher;

/// <summary>
/// Fetches the live FreePBX GraphQL introspection schema and writes it to disk.
/// </summary>
public class Program
{
    private const string IntrospectionQuery = """
        query IntrospectionQuery {
          __schema {
            queryType { name fields { name } }
            mutationType { name fields { name } }
            subscriptionType { name }
            types {
              kind
              name
              description
              fields { name description args { name type { kind name ofType { kind name ofType { kind name ofType { kind name } } } } } type { kind name ofType { kind name ofType { kind name ofType { kind name } } } } }
              inputFields { name description type { kind name ofType { kind name ofType { kind name } } } }
              enumValues { name description }
              interfaces { kind name }
              possibleTypes { kind name }
            }
          }
        }
        """;

    public static async Task<int> Main(string[] args)
    {
        Console.WriteLine("FreePBX 17 GraphQL Schema Fetcher");
        Console.WriteLine("=================================\n");

        var baseUrl = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("FREEPBX_BASE_URL");
        var outputPath = args.Length > 1 ? args[1] : "schema.json";

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            Console.Error.WriteLine("Usage: schema-fetcher <base-url> [output-path]");
            Console.Error.WriteLine("Or set FREEPBX_BASE_URL.");
            return 1;
        }

        var options = new FreepbxClientOptions(baseUrl)
        {
            ClientId = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_ID"),
            ClientSecret = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_SECRET"),
            Username = Environment.GetEnvironmentVariable("FREEPBX_USERNAME"),
            Password = Environment.GetEnvironmentVariable("FREEPBX_PASSWORD"),
            AccessToken = Environment.GetEnvironmentVariable("FREEPBX_ACCESS_TOKEN"),
            AllowInsecureCertificates = string.Equals(
                Environment.GetEnvironmentVariable("FREEPBX_ALLOW_INSECURE"),
                "true",
                StringComparison.OrdinalIgnoreCase)
        };

        using var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
        using var client = new FreepbxClient(options, loggerFactory);

        Console.WriteLine($"Fetching schema from {options.GraphQLUri}...");

        try
        {
            using var document = await client.GraphQL.ExecuteRawAsync(IntrospectionQuery);
            var json = JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(outputPath, json);

            var schema = document.RootElement.GetProperty("__schema");
            var queries = schema.GetProperty("queryType").GetProperty("fields").EnumerateArray()
                .Select(f => f.GetProperty("name").GetString()).ToList();
            var mutations = schema.GetProperty("mutationType").GetProperty("fields").EnumerateArray()
                .Select(f => f.GetProperty("name").GetString()).ToList();

            Console.WriteLine($"\nWrote introspection to {outputPath}");
            Console.WriteLine($"Queries ({queries.Count}): {string.Join(", ", queries)}");
            Console.WriteLine($"Mutations ({mutations.Count}): {string.Join(", ", mutations)}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
