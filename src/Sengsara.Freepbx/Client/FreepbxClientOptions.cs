using System;

namespace Sengsara.Freepbx.Client;

/// <summary>
/// Options for configuring the FreePBX client
/// </summary>
public class FreepbxClientOptions : Abstractions.Interfaces.FreepbxClientOptions
{
    /// <summary>
    /// Creates a new instance of FreepbxClientOptions
    /// </summary>
    public FreepbxClientOptions()
    {
    }

    /// <summary>
    /// Creates a new instance of FreepbxClientOptions with required endpoint
    /// </summary>
    /// <param name="endpoint">FreePBX GraphQL endpoint URL</param>
    public FreepbxClientOptions(string endpoint)
    {
        Endpoint = endpoint;
    }

    /// <summary>
    /// Validates the options
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required options are missing</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new ArgumentException("FreePBX endpoint is required", nameof(Endpoint));
        }

        if (TimeoutSeconds <= 0)
        {
            throw new ArgumentException("Timeout must be greater than zero", nameof(TimeoutSeconds));
        }

        if (MaxRetryAttempts < 0)
        {
            throw new ArgumentException("Max retry attempts cannot be negative", nameof(MaxRetryAttempts));
        }
    }
}