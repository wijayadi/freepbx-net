namespace Sengsara.Freepbx.Abstractions.Interfaces;

/// <summary>
/// Provides OAuth2 bearer tokens for the FreePBX API.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// Returns a valid access token, acquiring or refreshing it when required.
    /// </summary>
    /// <param name="forceRefresh">When true, ignores any cached token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<string> GetAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears any cached token, forcing the next call to acquire a new one.
    /// </summary>
    void Invalidate();
}
