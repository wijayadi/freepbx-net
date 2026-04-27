using Microsoft.Extensions.Logging;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;
using Sengsara.Freepbx.Abstractions.Models;
using Sengsara.Freepbx.Client;

namespace Sengsara.Freepbx.Services;

/// <summary>
/// Service for managing FreePBX extensions
/// </summary>
public class ExtensionService : IExtensionService
{
    private readonly FreepbxClient _client;
    private readonly ILogger<ExtensionService>? _logger;

    private const string GetAllQuery = @"
        query GetExtensions {
            extensions {
                id
                extension
                name
                email
                department
                description
                outboundCid
                deviceType
                userLevel
                createDate
                modifyDate
                enabled
            }
        }";

    private const string GetByIdQuery = @"
        query GetExtension($id: ID!) {
            extension(id: $id) {
                id
                extension
                name
                email
                department
                description
                outboundCid
                deviceType
                userLevel
                createDate
                modifyDate
                enabled
            }
        }";

    private const string CreateMutation = @"
        mutation CreateExtension($input: CreateExtensionInput!) {
            createExtension(input: $input) {
                id
                extension
                name
                email
                department
                description
                outboundCid
                deviceType
                userLevel
                createDate
                modifyDate
                enabled
            }
        }";

    private const string UpdateMutation = @"
        mutation UpdateExtension($id: ID!, $input: UpdateExtensionInput!) {
            updateExtension(id: $id, input: $input) {
                id
                extension
                name
                email
                department
                description
                outboundCid
                deviceType
                userLevel
                createDate
                modifyDate
                enabled
            }
        }";

    private const string DeleteMutation = @"
        mutation DeleteExtension($id: ID!) {
            deleteExtension(id: $id)
        }";

    public ExtensionService(FreepbxClient client, ILogger<ExtensionService>? logger = null)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExtensionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Retrieving all extensions");

        var response = await _client.GraphQL.ExecuteQueryAsync<ExtensionsResponse>(
            GetAllQuery,
            cancellationToken: cancellationToken);

        return response.Extensions ?? [];
    }

    /// <inheritdoc />
    public async Task<ExtensionDto?> GetByIdAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Retrieving extension with ID: {ExtensionId}", extensionId);

        var response = await _client.GraphQL.ExecuteQueryAsync<ExtensionResponse>(
            GetByIdQuery,
            new Dictionary<string, object?> { ["id"] = extensionId },
            cancellationToken);

        return response.Extension;
    }

    /// <inheritdoc />
    public async Task<ExtensionDto> CreateAsync(CreateExtensionRequest request, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Creating extension: {Extension}", request.Extension);

        var input = new Dictionary<string, object?>
        {
            ["extension"] = request.Extension,
            ["name"] = request.Name,
            ["email"] = request.Email,
            ["department"] = request.Department,
            ["description"] = request.Description,
            ["outboundCid"] = request.OutboundCid
        };

        var response = await _client.GraphQL.ExecuteMutationAsync<CreateExtensionResponse>(
            CreateMutation,
            new Dictionary<string, object?> { ["input"] = input },
            cancellationToken);

        _logger?.LogInformation("Extension created with ID: {ExtensionId}", response.CreateExtension.Id);

        return response.CreateExtension;
    }

    /// <inheritdoc />
    public async Task<ExtensionDto> UpdateAsync(string extensionId, UpdateExtensionRequest request, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Updating extension: {ExtensionId}", extensionId);

        var input = new Dictionary<string, object?>();

        if (request.Name != null) input["name"] = request.Name;
        if (request.Email != null) input["email"] = request.Email;
        if (request.Department != null) input["department"] = request.Department;
        if (request.Description != null) input["description"] = request.Description;
        if (request.OutboundCid != null) input["outboundCid"] = request.OutboundCid;

        var response = await _client.GraphQL.ExecuteMutationAsync<UpdateExtensionResponse>(
            UpdateMutation,
            new Dictionary<string, object?>
            {
                ["id"] = extensionId,
                ["input"] = input
            },
            cancellationToken);

        _logger?.LogInformation("Extension updated: {ExtensionId}", extensionId);

        return response.UpdateExtension;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string extensionId, CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Deleting extension: {ExtensionId}", extensionId);

        var response = await _client.GraphQL.ExecuteMutationAsync<DeleteExtensionResponse>(
            DeleteMutation,
            new Dictionary<string, object?> { ["id"] = extensionId },
            cancellationToken);

        _logger?.LogInformation("Extension deleted: {ExtensionId}, Result: {Result}", extensionId, response.DeleteExtension);

        return response.DeleteExtension;
    }

    // Response DTOs
    private class ExtensionsResponse
    {
        public List<ExtensionDto>? Extensions { get; set; }
    }

    private class ExtensionResponse
    {
        public ExtensionDto? Extension { get; set; }
    }

    private class CreateExtensionResponse
    {
        public ExtensionDto CreateExtension { get; set; } = new();
    }

    private class UpdateExtensionResponse
    {
        public ExtensionDto UpdateExtension { get; set; } = new();
    }

    private class DeleteExtensionResponse
    {
        public bool DeleteExtension { get; set; }
    }
}