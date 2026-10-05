using Sengsara.Freepbx.Abstractions.Models;
using Xunit;

namespace Sengsara.Freepbx.IntegrationTests.Extensions;

[Collection("FreePbx")]
public class ExtensionIntegrationTests
{
    private readonly FreePbxFixture _fixture;

    public ExtensionIntegrationTests(FreePbxFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestConnection_ShouldSucceed()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        Assert.True(await _fixture.Client!.TestConnectionAsync());
    }

    [Fact]
    public async Task GetAllExtensions_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var extensions = await _fixture.Client!.Extensions.GetAllAsync();

        Assert.NotNull(extensions);
        Assert.All(extensions, e => Assert.False(string.IsNullOrWhiteSpace(e.ExtensionId)));
    }

    [Fact]
    public async Task GetExtension_ForFirstExtension_ShouldReturnIt()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var all = await _fixture.Client!.Extensions.GetAllAsync();
        var first = all.FirstOrDefault();
        if (first is null)
        {
            return;
        }

        var extension = await _fixture.Client.Extensions.GetByIdAsync(first.ExtensionId);

        Assert.NotNull(extension);
        Assert.Equal(first.ExtensionId, extension!.ExtensionId);
    }

    [Fact]
    public async Task CreateAndDeleteExtension_ShouldRoundTrip()
    {
        if (!_fixture.IsAvailable || !FreepbxTestEnvironment.WriteTestsEnabled)
        {
            return;
        }

        var client = _fixture.Client!;
        var extensionId = "8791";

        // Ensure the extension does not already exist.
        var existing = await client.Extensions.GetByIdAsync(extensionId);
        if (existing is not null)
        {
            return;
        }

        try
        {
            var created = await client.Extensions.CreateAsync(new AddExtensionRequest
            {
                ExtensionId = extensionId,
                Name = "Integration Test",
                Email = "integration@example.com",
                Tech = "pjsip",
                VmEnable = true,
                VmPassword = extensionId
            });

            Assert.True(created.Success, created.Message);

            var fetched = await client.Extensions.GetByIdAsync(extensionId);
            Assert.NotNull(fetched);
        }
        finally
        {
            var deleted = await client.Extensions.DeleteAsync(extensionId);
            Assert.True(deleted.Success, deleted.Message);
        }
    }
}
