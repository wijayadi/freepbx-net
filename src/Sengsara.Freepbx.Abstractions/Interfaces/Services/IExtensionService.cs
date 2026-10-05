using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces.Services;

/// <summary>
/// Service for managing FreePBX Core extensions.
/// </summary>
public interface IExtensionService
{
    /// <summary>Gets all extensions.</summary>
    Task<IReadOnlyList<ExtensionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets all valid extensions (those that are not disabled/incomplete).</summary>
    Task<IReadOnlyList<ExtensionDto>> GetAllValidAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets an extension by its number.</summary>
    Task<ExtensionDto?> GetByIdAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new extension.</summary>
    Task<MutationResult> CreateAsync(AddExtensionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing extension.</summary>
    Task<MutationResult> UpdateAsync(UpdateExtensionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes an extension.</summary>
    Task<MutationResult> DeleteAsync(string extensionId, CancellationToken cancellationToken = default);

    /// <summary>Creates a range of extensions.</summary>
    Task<MutationResult> CreateRangeAsync(CreateExtensionRangeRequest request, CancellationToken cancellationToken = default);
}
