using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX Core users through GraphQL.
/// </summary>
public sealed class CoreUserService : GraphQLServiceBase, ICoreUserService
{
    private const string GetAllQuery = @"
        query GetCoreUsers {
          allCoreUsers {
            coreUser { " + GraphQLFields.CoreUser + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetCoreUser($id: ID) {
          coreUser(id: $id) { " + GraphQLFields.CoreUser + @" }
        }";

    private const string CreateMutation = @"
        mutation AddCoreUser($input: addCoreUserInput!) {
          addCoreUser(input: $input) {
            coreUser { " + GraphQLFields.CoreUser + @" }
          }
        }";

    private const string UpdateMutation = @"
        mutation UpdateCoreUser($input: updateCoreUserInput!) {
          updateCoreUser(input: $input) {
            coreuser { " + GraphQLFields.CoreUser + @" }
          }
        }";

    private const string DeleteMutation = @"
        mutation RemoveCoreUser($input: removeCoreUserInput!) {
          removeCoreUser(input: $input) { deletedId }
        }";

    /// <summary>
    /// Creates a new core user service.
    /// </summary>
    public CoreUserService(IGraphQLExecutor graphQL, ILogger<CoreUserService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CoreUserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<CoreUsersResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.AllCoreUsers?.CoreUser ?? [];
    }

    /// <inheritdoc />
    public async Task<CoreUserDto?> GetByIdAsync(string extension, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        var result = await GraphQL.TryExecuteQueryAsync<CoreUserResponse>(
            GetByIdQuery,
            new { id = extension },
            cancellationToken).ConfigureAwait(false);

        var user = result.Data?.CoreUser;
        return string.IsNullOrEmpty(user?.Extension) ? null : user;
    }

    /// <inheritdoc />
    public async Task<CoreUserDto?> CreateAsync(AddCoreUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<AddCoreUserResponse>(
            CreateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.AddCoreUser?.CoreUser;
    }

    /// <inheritdoc />
    public async Task<CoreUserDto?> UpdateAsync(UpdateCoreUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<UpdateCoreUserResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.UpdateCoreUser?.CoreUser;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string extension, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        var response = await GraphQL.ExecuteMutationAsync<RemoveCoreUserResponse>(
            DeleteMutation,
            new { input = new { extension } },
            cancellationToken).ConfigureAwait(false);

        return !string.IsNullOrEmpty(response.RemoveCoreUser?.DeletedId);
    }

    private sealed class CoreUsersResponse
    {
        public CoreUserConnection? AllCoreUsers { get; set; }
    }

    private sealed class CoreUserConnection
    {
        public List<CoreUserDto>? CoreUser { get; set; }
    }

    private sealed class CoreUserResponse
    {
        public CoreUserDto? CoreUser { get; set; }
    }

    private sealed class AddCoreUserResponse
    {
        public CoreUserPayload? AddCoreUser { get; set; }
    }

    private sealed class UpdateCoreUserResponse
    {
        public CoreUserPayload? UpdateCoreUser { get; set; }
    }

    private sealed class CoreUserPayload
    {
        public CoreUserDto? CoreUser { get; set; }
    }

    private sealed class RemoveCoreUserResponse
    {
        public DeletePayload? RemoveCoreUser { get; set; }
    }

    private sealed class DeletePayload
    {
        public string? DeletedId { get; set; }
    }
}

/// <summary>
/// Service for managing FreePBX Core devices through GraphQL.
/// </summary>
public sealed class CoreDeviceService : GraphQLServiceBase, ICoreDeviceService
{
    private const string GetAllQuery = @"
        query GetCoreDevices {
          fetchAllCoreDevices {
            totalCount
            coreDevice { " + GraphQLFields.CoreDevice + @" }
          }
        }";

    private const string GetByIdQuery = @"
        query GetCoreDevice($device_id: ID) {
          fetchCoreDevice(device_id: $device_id) { " + GraphQLFields.CoreDevice + @" }
        }";

    private const string CreateMutation = @"
        mutation AddCoreDevice($input: addCoreDeviceInput!) {
          addCoreDevice(input: $input) {
            status
            message
            coreDevice { " + GraphQLFields.CoreDevice + @" }
          }
        }";

    private const string UpdateMutation = @"
        mutation UpdateCoreDevice($input: updateCoreDeviceInput!) {
          updateCoreDevice(input: $input) {
            status
            message
            coreDevice { " + GraphQLFields.CoreDevice + @" }
          }
        }";

    private const string DeleteMutation = @"
        mutation DeleteCoreDevice($input: deleteCoreDeviceInput!) {
          deleteCoreDevice(input: $input) { status message deletedId }
        }";

    /// <summary>
    /// Creates a new core device service.
    /// </summary>
    public CoreDeviceService(IGraphQLExecutor graphQL, ILogger<CoreDeviceService> logger)
        : base(graphQL, logger)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CoreDeviceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await GraphQL.ExecuteQueryAsync<CoreDevicesResponse>(GetAllQuery, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.FetchAllCoreDevices?.CoreDevice ?? [];
    }

    /// <inheritdoc />
    public async Task<CoreDeviceDto?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);

        var result = await GraphQL.TryExecuteQueryAsync<CoreDeviceResponse>(
            GetByIdQuery,
            new { device_id = deviceId },
            cancellationToken).ConfigureAwait(false);

        var device = result.Data?.FetchCoreDevice;
        return string.IsNullOrEmpty(device?.DeviceId) ? null : device;
    }

    /// <inheritdoc />
    public async Task<CoreDeviceDto?> CreateAsync(AddCoreDeviceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<CoreDevicePayloadResponse>(
            CreateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.AddCoreDevice?.CoreDevice;
    }

    /// <inheritdoc />
    public async Task<CoreDeviceDto?> UpdateAsync(UpdateCoreDeviceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await GraphQL.ExecuteMutationAsync<CoreDevicePayloadResponse>(
            UpdateMutation,
            new { input = request },
            cancellationToken).ConfigureAwait(false);

        return response.UpdateCoreDevice?.CoreDevice;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);

        var response = await GraphQL.ExecuteMutationAsync<DeleteCoreDeviceResponse>(
            DeleteMutation,
            new { input = new { id = deviceId } },
            cancellationToken).ConfigureAwait(false);

        return response.DeleteCoreDevice?.Status ?? false;
    }

    private sealed class CoreDevicesResponse
    {
        public CoreDeviceConnection? FetchAllCoreDevices { get; set; }
    }

    private sealed class CoreDeviceConnection
    {
        public int? TotalCount { get; set; }

        public List<CoreDeviceDto>? CoreDevice { get; set; }
    }

    private sealed class CoreDeviceResponse
    {
        public CoreDeviceDto? FetchCoreDevice { get; set; }
    }

    private sealed class CoreDevicePayloadResponse
    {
        public CoreDevicePayload? AddCoreDevice { get; set; }

        public CoreDevicePayload? UpdateCoreDevice { get; set; }
    }

    private sealed class CoreDevicePayload : MutationPayload
    {
        public CoreDeviceDto? CoreDevice { get; set; }
    }

    private sealed class DeleteCoreDeviceResponse
    {
        public DeleteCoreDevicePayload? DeleteCoreDevice { get; set; }
    }

    private sealed class DeleteCoreDevicePayload : MutationPayload
    {
        public string? DeletedId { get; set; }
    }
}
