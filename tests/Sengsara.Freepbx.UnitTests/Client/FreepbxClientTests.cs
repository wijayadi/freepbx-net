using Xunit;
using Sengsara.Freepbx.Client;
using Microsoft.Extensions.Logging;

namespace Sengsara.Freepbx.UnitTests.Client;

public class FreepbxClientTests
{
    [Fact]
    public void Constructor_WithValidOptions_ShouldCreateClient()
    {
        // Arrange
        var options = new FreepbxClientOptions("https://freepbx.example.com/graphql")
        {
            ApiKey = "test-api-key",
            TimeoutSeconds = 30
        };

        // Act
        var client = new FreepbxClient(options);

        // Assert
        Assert.NotNull(client);
        Assert.NotNull(client.GraphQL);
        Assert.NotNull(client.Extensions);
        Assert.NotNull(client.Queues);
        Assert.Equal(options, client.Options);
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FreepbxClient(null!));
    }

    [Fact]
    public void Constructor_WithEmptyEndpoint_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new FreepbxClientOptions
        {
            Endpoint = "",
            ApiKey = "test-api-key"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new FreepbxClient(options));
    }

    [Fact]
    public void Constructor_WithZeroTimeout_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new FreepbxClientOptions("https://freepbx.example.com")
        {
            TimeoutSeconds = 0
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new FreepbxClient(options));
    }

    [Fact]
    public void Options_Validate_WithValidOptions_ShouldNotThrow()
    {
        // Arrange
        var options = new FreepbxClientOptions("https://freepbx.example.com")
        {
            TimeoutSeconds = 30,
            MaxRetryAttempts = 3
        };

        // Act & Assert - should not throw
        options.Validate();
    }
}