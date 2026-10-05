using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Abstractions.Interfaces.Services;

/// <summary>
/// Service for managing FreePBX Core users.
/// </summary>
public interface ICoreUserService
{
    /// <summary>Gets all core users.</summary>
    Task<IReadOnlyList<CoreUserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a core user by extension.</summary>
    Task<CoreUserDto?> GetByIdAsync(string extension, CancellationToken cancellationToken = default);

    /// <summary>Creates a core user.</summary>
    Task<CoreUserDto?> CreateAsync(AddCoreUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates a core user.</summary>
    Task<CoreUserDto?> UpdateAsync(UpdateCoreUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>Removes a core user.</summary>
    Task<bool> DeleteAsync(string extension, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for managing FreePBX Core devices.
/// </summary>
public interface ICoreDeviceService
{
    /// <summary>Gets all core devices.</summary>
    Task<IReadOnlyList<CoreDeviceDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a core device by id.</summary>
    Task<CoreDeviceDto?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default);

    /// <summary>Creates a core device.</summary>
    Task<CoreDeviceDto?> CreateAsync(AddCoreDeviceRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates a core device.</summary>
    Task<CoreDeviceDto?> UpdateAsync(UpdateCoreDeviceRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes a core device.</summary>
    Task<bool> DeleteAsync(string deviceId, CancellationToken cancellationToken = default);
}
