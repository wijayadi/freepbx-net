using System.Net;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Sengsara.Freepbx;
using Sengsara.Freepbx.Abstractions.Models;
using Sengsara.Freepbx.GraphQL;
using Sengsara.Freepbx.Rest;
using Sengsara.Freepbx.Services;
using Xunit;

namespace Sengsara.Freepbx.UnitTests.Services;

/// <summary>
/// A test double that returns canned HTTP responses and records the requests.
/// </summary>
internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, string, HttpResponseMessage> _responder;

    public List<string> Requests { get; } = [];

    public List<string> RequestUris { get; } = [];

    public StubHttpMessageHandler(Func<HttpRequestMessage, string, HttpResponseMessage> responder)
    {
        _responder = responder;
    }

    public static StubHttpMessageHandler Json(string json, HttpStatusCode status = HttpStatusCode.OK)
        => new((_, _) => new HttpResponseMessage(status)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add(body);
        RequestUris.Add(request.RequestUri?.ToString() ?? string.Empty);
        return _responder(request, body);
    }
}

public class ExtensionServiceTests
{
    private static (ExtensionService Service, StubHttpMessageHandler Handler) CreateService(string json)
    {
        var handler = StubHttpMessageHandler.Json(json);
        var options = new FreepbxClientOptions("https://freepbx.example.com") { AccessToken = "token" };
        var http = new HttpClient(handler);
        var executor = new GraphQLExecutor(http, options, NullLogger<GraphQLExecutor>.Instance);
        return (new ExtensionService(executor, NullLogger<ExtensionService>.Instance), handler);
    }

    [Fact]
    public async Task GetAllAsync_ShouldMapConnection()
    {
        const string json = """
            {"data":{"fetchAllExtensions":{"totalCount":1,"count":1,"extension":[
              {"id":"x","extensionId":"101","tech":"pjsip",
               "user":{"extension":"101","name":"Alice","callwaiting":true,"donotdisturb":false},
               "coreDevice":{"deviceId":"101","tech":"pjsip","dial":"PJSIP/101","devicetype":"fixed"}}]}}}
            """;

        var (service, handler) = CreateService(json);

        var result = await service.GetAllAsync();

        var extension = Assert.Single(result);
        Assert.Equal("101", extension.ExtensionId);
        Assert.Equal("pjsip", extension.Tech);
        Assert.Equal("Alice", extension.Name);
        Assert.Equal("PJSIP/101", extension.CoreDevice!.Dial);
        Assert.Contains("fetchAllExtensions", handler.Requests[0]);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldPassExtensionVariable()
    {
        const string json = """{"data":{"fetchExtension":{"extensionId":"101","tech":"pjsip"}}}""";
        var (service, handler) = CreateService(json);

        var result = await service.GetByIdAsync("101");

        Assert.NotNull(result);
        Assert.Equal("101", result!.ExtensionId);
        Assert.Contains("\"extensionId\":\"101\"", handler.Requests[0]);
    }

    [Fact]
    public async Task CreateAsync_ShouldMapMutationResult()
    {
        const string json = """{"data":{"addExtension":{"status":true,"message":"Extension has been created Successfully"}}}""";
        var (service, handler) = CreateService(json);

        var result = await service.CreateAsync(new AddExtensionRequest
        {
            ExtensionId = "101",
            Name = "Alice",
            Email = "alice@example.com"
        });

        Assert.True(result.Success);
        Assert.Contains("Extension has been created", result.Message);
        Assert.Contains("addExtension", handler.Requests[0]);
        Assert.Contains("\"extensionId\":\"101\"", handler.Requests[0]);
    }
}

public class QueueServiceTests
{
    private static (QueueService Service, StubHttpMessageHandler Handler) CreateService(string json)
    {
        var handler = StubHttpMessageHandler.Json(json);
        var options = new FreepbxClientOptions("https://freepbx.example.com") { AccessToken = "token" };
        var http = new HttpClient(handler);
        var rest = new FreepbxRestClient(http, options);
        return (new QueueService(rest, NullLogger<QueueService>.Instance), handler);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnQueuesFromDictionary()
    {
        const string json = """{"1001":{"extension":"1001","name":"Support"},"1002":{"extension":"1002","name":"Sales"}}""";
        var (service, handler) = CreateService(json);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("1001", result[0].Extension);
        Assert.Equal("Support", result[0].Name);
        Assert.Contains("/admin/api/api/rest/queues/", handler.RequestUris[0]);
    }

    [Fact]
    public async Task GetMembersAsync_ShouldMapStaticAndDynamic()
    {
        const string json = """{"dynmembers":["110"],"member":["101","102"]}""";
        var (service, _) = CreateService(json);

        var result = await service.GetMembersAsync("1001");

        Assert.Equal(["101", "102"], result.Static);
        Assert.Equal(["110"], result.Dynamic);
    }

    [Fact]
    public async Task GetByIdAsync_WithFalseResponse_ShouldReturnNull()
    {
        var (service, _) = CreateService("false");

        var result = await service.GetByIdAsync("9999");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetMembersAsync_ShouldSendNewlineSeparatedBody()
    {
        var (service, handler) = CreateService("true");

        var result = await service.SetMembersAsync("1001", new QueueMembers
        {
            Static = ["101", "102"],
            Dynamic = ["110"]
        });

        Assert.True(result);
        Assert.Contains("101\\n102", handler.Requests[0]);
        Assert.Contains("dynmembers", handler.Requests[0]);
    }
}
