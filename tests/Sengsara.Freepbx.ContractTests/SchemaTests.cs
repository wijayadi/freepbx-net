using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Client;
using Xunit;

namespace Sengsara.Freepbx.ContractTests;

/// <summary>
/// Validates that the live FreePBX GraphQL schema exposes the operations this
/// library depends on. Tests no-op when no FreePBX instance is configured via
/// environment variables.
/// </summary>
public class SchemaTests
{
    private const string IntrospectionQuery = @"
        query {
          __schema {
            queryType { fields { name } }
            mutationType { fields { name } }
          }
        }";

    private static readonly string[] ExpectedQueries =
    [
        "fetchAllExtensions", "fetchAllValidExtensions", "fetchExtension",
        "allCoreUsers", "coreUser",
        "fetchAllCoreDevices", "fetchCoreDevice",
        "fetchAllRingGroups", "fetchRingGroup",
        "allInboundRoutes", "inboundRoute",
        "fetchAllRecordings",
        "allMusiconholds", "musiconhold",
        "fetchVoiceMail", "fetchFollowMe",
        "fetchAllCdrs", "fetchCdr"
    ];

    private static readonly string[] ExpectedMutations =
    [
        "addExtension", "updateExtension", "deleteExtension",
        "addCoreUser", "updateCoreUser", "removeCoreUser",
        "addCoreDevice", "updateCoreDevice", "deleteCoreDevice",
        "addRingGroup", "updateRingGroup", "deleteRingGroup",
        "addInboundRoute", "updateInboundRoute", "removeInboundRoute",
        "addRecording", "updateRecording", "deleteRecording",
        "enableVoiceMail", "disableVoiceMail",
        "updateFollowMe", "enableFollowMe", "disableFollowMe"
    ];

    private static bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FREEPBX_BASE_URL")) &&
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FREEPBX_CLIENT_ID"));

    [Fact]
    public async Task Schema_ShouldContainRequiredQueries()
    {
        var schema = await GetSchemaAsync();
        if (schema is null)
        {
            return;
        }

        foreach (var expected in ExpectedQueries)
        {
            Assert.Contains(expected, schema.Value.QueryFields);
        }
    }

    [Fact]
    public async Task Schema_ShouldContainRequiredMutations()
    {
        var schema = await GetSchemaAsync();
        if (schema is null)
        {
            return;
        }

        foreach (var expected in ExpectedMutations)
        {
            Assert.Contains(expected, schema.Value.MutationFields);
        }
    }

    private static async Task<(HashSet<string> QueryFields, HashSet<string> MutationFields)?> GetSchemaAsync()
    {
        if (!IsConfigured)
        {
            return null;
        }

        var options = new FreepbxClientOptions(Environment.GetEnvironmentVariable("FREEPBX_BASE_URL")!)
        {
            ClientId = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_ID"),
            ClientSecret = Environment.GetEnvironmentVariable("FREEPBX_CLIENT_SECRET"),
            AccessToken = Environment.GetEnvironmentVariable("FREEPBX_ACCESS_TOKEN"),
            AllowInsecureCertificates = string.Equals(
                Environment.GetEnvironmentVariable("FREEPBX_ALLOW_INSECURE"),
                "true",
                StringComparison.OrdinalIgnoreCase)
        };

        using var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
        using var client = new FreepbxClient(options, loggerFactory);

        using var document = await client.GraphQL.ExecuteRawAsync(IntrospectionQuery);
        var schema = document.RootElement.GetProperty("__schema");

        var queries = schema.GetProperty("queryType").GetProperty("fields")
            .EnumerateArray().Select(f => f.GetProperty("name").GetString()!).ToHashSet();
        var mutations = schema.GetProperty("mutationType").GetProperty("fields")
            .EnumerateArray().Select(f => f.GetProperty("name").GetString()!).ToHashSet();

        return (queries, mutations);
    }
}
