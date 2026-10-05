using System.Text.Json;
using Sengsara.Freepbx.Client;
using Microsoft.Extensions.Logging;

namespace Sengsara.Freepbx.IntegrationTests;

/// <summary>
/// Reads FreePBX connection settings from environment variables or an optional
/// <c>freepbx.test.json</c> file located in the repository root. The file is
/// git-ignored and is intended for local development only.
/// </summary>
public static class FreepbxTestEnvironment
{
    private static readonly Dictionary<string, string?> FileValues = LoadFile();

    public static string? BaseUrl => Get("FREEPBX_BASE_URL", "BaseUrl");
    public static string? ClientId => Get("FREEPBX_CLIENT_ID", "ClientId");
    public static string? ClientSecret => Get("FREEPBX_CLIENT_SECRET", "ClientSecret");
    public static string? Username => Get("FREEPBX_USERNAME", "Username");
    public static string? Password => Get("FREEPBX_PASSWORD", "Password");
    public static string? AccessToken => Get("FREEPBX_ACCESS_TOKEN", "AccessToken");

    public static bool AllowInsecure =>
        string.Equals(Get("FREEPBX_ALLOW_INSECURE", "AllowInsecureCertificates"), "true", StringComparison.OrdinalIgnoreCase);

    public static bool WriteTestsEnabled =>
        string.Equals(Get("FREEPBX_ENABLE_WRITE_TESTS", "EnableWriteTests"), "true", StringComparison.OrdinalIgnoreCase);

    public static bool IsConfigured =>
        !string.IsNullOrWhiteSpace(BaseUrl) &&
        (!string.IsNullOrWhiteSpace(AccessToken) ||
         (!string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret)));

    public static FreepbxClientOptions CreateOptions() => new(BaseUrl!)
    {
        ClientId = ClientId,
        ClientSecret = ClientSecret,
        Username = Username,
        Password = Password,
        AccessToken = AccessToken,
        AllowInsecureCertificates = AllowInsecure,
        TimeoutSeconds = 60
    };

    public static FreepbxClient CreateClient()
    {
        var factory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Warning);
        });

        return new FreepbxClient(CreateOptions(), factory);
    }

    private static string? Get(string envName, string fileKey)
    {
        var env = Environment.GetEnvironmentVariable(envName);
        if (!string.IsNullOrWhiteSpace(env))
        {
            return env;
        }

        return FileValues.TryGetValue(fileKey, out var value) && !string.IsNullOrWhiteSpace(value) ? value : null;
    }

    private static Dictionary<string, string?> LoadFile()
    {
        var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var path = FindFile("freepbx.test.json");
        if (path is null)
        {
            return values;
        }

        try
        {
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            foreach (var property in document.RootElement.EnumerateObject())
            {
                values[property.Name] = property.Value.ValueKind == JsonValueKind.String
                    ? property.Value.GetString()
                    : property.Value.ToString();
            }
        }
        catch
        {
            // Ignore malformed local configuration files.
        }

        return values;
    }

    private static string? FindFile(string fileName)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            if (File.Exists(Path.Combine(directory.FullName, "Sengsara.Freepbx.sln")))
            {
                break;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
