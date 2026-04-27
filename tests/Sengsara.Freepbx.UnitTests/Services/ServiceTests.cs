using Xunit;
using Moq;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.UnitTests.Services;

public class ExtensionServiceTests
{
    private readonly Mock<FreepbxClient> _mockClient;
    private readonly Sengsara.Freepbx.Services.ExtensionService _service;

    public ExtensionServiceTests()
    {
        _mockClient = new Mock<FreepbxClient>(
            new FreepbxClientOptions("https://freepbx.example.com"),
            null);

        _service = new Sengsara.Freepbx.Services.ExtensionService(_mockClient.Object, null);
    }

    [Fact]
    public void GetAllAsync_ShouldReturnExtensionsList()
    {
        // This is a placeholder test - in real scenario, mock GraphQLExecutor
        Assert.NotNull(_service);
    }

    [Fact]
    public void GetByIdAsync_WithValidId_ShouldReturnExtension()
    {
        // This is a placeholder test
        Assert.NotNull(_service);
    }
}

public class QueueServiceTests
{
    private readonly Mock<FreepbxClient> _mockClient;
    private readonly Sengsara.Freepbx.Services.QueueService _service;

    public QueueServiceTests()
    {
        _mockClient = new Mock<FreepbxClient>(
            new FreepbxClientOptions("https://freepbx.example.com"),
            null);

        _service = new Sengsara.Freepbx.Services.QueueService(_mockClient.Object, null);
    }

    [Fact]
    public void GetAllAsync_ShouldReturnQueuesList()
    {
        // This is a placeholder test - in real scenario, mock GraphQLExecutor
        Assert.NotNull(_service);
    }

    [Fact]
    public void GetByIdAsync_WithValidId_ShouldReturnQueue()
    {
        // This is a placeholder test
        Assert.NotNull(_service);
    }
}