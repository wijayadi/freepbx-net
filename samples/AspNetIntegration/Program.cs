using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Abstractions.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FreePBX services from configuration (appsettings.json "FreePbx" section).
builder.Services.AddFreePbxFromConfiguration(builder.Configuration);

// Register sample services
builder.Services.AddScoped<ExtensionEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// Sample service demonstrating FreePBX integration.
/// </summary>
public class ExtensionEndpoints
{
    private readonly IExtensionService _extensionService;
    private readonly IQueueService _queueService;
    private readonly ILogger<ExtensionEndpoints> _logger;

    public ExtensionEndpoints(
        IExtensionService extensionService,
        IQueueService queueService,
        ILogger<ExtensionEndpoints> logger)
    {
        _extensionService = extensionService;
        _queueService = queueService;
        _logger = logger;
    }

    /// <summary>Get all extensions.</summary>
    public async Task<IResult> GetExtensions()
    {
        try
        {
            var extensions = await _extensionService.GetAllAsync();
            return Results.Ok(extensions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving extensions");
            return Results.Problem("Error retrieving extensions");
        }
    }

    /// <summary>Get all queues.</summary>
    public async Task<IResult> GetQueues()
    {
        try
        {
            var queues = await _queueService.GetAllAsync();
            return Results.Ok(queues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving queues");
            return Results.Problem("Error retrieving queues");
        }
    }
}
