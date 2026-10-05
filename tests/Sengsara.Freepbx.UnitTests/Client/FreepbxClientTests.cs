using Sengsara.Freepbx;
using Sengsara.Freepbx.Client;
using Xunit;

namespace Sengsara.Freepbx.UnitTests.Client;

public class FreepbxClientTests
{
    private static FreepbxClientOptions ValidOptions() => new("https://freepbx.example.com")
    {
        ClientId = "client-id",
        ClientSecret = "client-secret"
    };

    [Fact]
    public void Constructor_WithValidOptions_ShouldCreateClientAndServices()
    {
        var options = ValidOptions();

        using var client = new FreepbxClient(options);

        Assert.NotNull(client);
        Assert.NotNull(client.GraphQL);
        Assert.NotNull(client.Extensions);
        Assert.NotNull(client.CoreUsers);
        Assert.NotNull(client.CoreDevices);
        Assert.NotNull(client.RingGroups);
        Assert.NotNull(client.InboundRoutes);
        Assert.NotNull(client.Recordings);
        Assert.NotNull(client.MusicOnHold);
        Assert.NotNull(client.VoiceMail);
        Assert.NotNull(client.FollowMe);
        Assert.NotNull(client.Cdrs);
        Assert.NotNull(client.Queues);
        Assert.Same(options, client.Options);
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new FreepbxClient(null!));
    }

    [Fact]
    public void Constructor_WithEmptyBaseUrl_ShouldThrowArgumentException()
    {
        var options = new FreepbxClientOptions
        {
            BaseUrl = "",
            ClientId = "client-id",
            ClientSecret = "client-secret"
        };

        Assert.Throws<ArgumentException>(() => new FreepbxClient(options));
    }

    [Fact]
    public void Constructor_WithoutAuthentication_ShouldThrowArgumentException()
    {
        var options = new FreepbxClientOptions("https://freepbx.example.com");

        Assert.Throws<ArgumentException>(() => new FreepbxClient(options));
    }

    [Fact]
    public void Constructor_WithZeroTimeout_ShouldThrowArgumentException()
    {
        var options = ValidOptions();
        options.TimeoutSeconds = 0;

        Assert.Throws<ArgumentException>(() => new FreepbxClient(options));
    }

    [Fact]
    public void Options_BuildUris_ShouldPreserveBasePath()
    {
        var options = new FreepbxClientOptions("https://host/freepbx")
        {
            ClientId = "id",
            ClientSecret = "secret"
        };

        Assert.Equal("https://host/freepbx/admin/api/api/gql", options.GraphQLUri.ToString());
        Assert.Equal("https://host/freepbx/admin/api/api/rest", options.RestBaseUri.ToString());
        Assert.Equal("https://host/freepbx/admin/api/api/token", options.TokenUri.ToString());
    }

    [Fact]
    public void Options_Validate_WithStaticToken_ShouldNotThrow()
    {
        var options = new FreepbxClientOptions("https://freepbx.example.com")
        {
            AccessToken = "a-token",
            TimeoutSeconds = 30,
            MaxRetryAttempts = 3
        };

        options.Validate();
    }

    [Fact]
    public void Options_Validate_WithNegativeRetry_ShouldThrow()
    {
        var options = ValidOptions();
        options.MaxRetryAttempts = -1;

        Assert.Throws<ArgumentException>(() => options.Validate());
    }
}
