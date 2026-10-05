using Sengsara.Freepbx.Client;
using Xunit;

namespace Sengsara.Freepbx.IntegrationTests;

/// <summary>
/// Shared FreePBX client for integration tests. Tests no-op when the environment
/// is not configured.
/// </summary>
public sealed class FreePbxFixture : IDisposable
{
    public FreePbxFixture()
    {
        IsAvailable = FreepbxTestEnvironment.IsConfigured;
        if (IsAvailable)
        {
            Client = FreepbxTestEnvironment.CreateClient();
        }
    }

    public bool IsAvailable { get; }

    public FreepbxClient? Client { get; }

    public void Dispose() => Client?.Dispose();
}

[CollectionDefinition("FreePbx")]
public sealed class FreePbxCollection : ICollectionFixture<FreePbxFixture>
{
}
