using System.Text.Json;
using Sengsara.Freepbx.Abstractions.Models;
using Xunit;

namespace Sengsara.Freepbx.UnitTests.Serialization;

public class SerializationTests
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void ExtensionDto_ShouldDeserialize_FromGraphQLPayload()
    {
        const string json = """
            {"id":"x","extensionId":"101","tech":"pjsip",
             "user":{"extension":"101","name":"Alice","donotdisturb":false},
             "coreDevice":{"deviceId":"101","tech":"pjsip","dial":"PJSIP/101","devicetype":"fixed"}}
            """;

        var dto = JsonSerializer.Deserialize<ExtensionDto>(json, Options)!;

        Assert.Equal("101", dto.ExtensionId);
        Assert.Equal("pjsip", dto.Tech);
        Assert.Equal("Alice", dto.User!.Name);
        Assert.Equal("PJSIP/101", dto.CoreDevice!.Dial);
    }

    [Fact]
    public void CoreUserDto_ShouldDeserialize_UnderscoredFields()
    {
        const string json = """
            {"id":"y","extension":"101","callwaiting":true,
             "recording_in_external":"dontcare","recording_out_external":"never",
             "recording_ondemand":"disabled","recording_priority":10,
             "callforward_unconditional":"","donotdisturb":false}
            """;

        var dto = JsonSerializer.Deserialize<CoreUserDto>(json, Options)!;

        Assert.Equal("101", dto.Extension);
        Assert.Equal("dontcare", dto.RecordingInExternal);
        Assert.Equal("never", dto.RecordingOutExternal);
        Assert.Equal(10, dto.RecordingPriority);
        Assert.True(dto.CallWaiting);
    }

    [Fact]
    public void AddExtensionRequest_ShouldSerialize_UsingGraphQLFieldNames()
    {
        var request = new AddExtensionRequest
        {
            ExtensionId = "101",
            Name = "Alice",
            Email = "alice@example.com",
            Tech = "pjsip",
            VmEnable = true,
            VmPassword = "1234"
        };

        var json = JsonSerializer.Serialize(request, Options);

        Assert.Contains("\"extensionId\":\"101\"", json);
        Assert.Contains("\"vmEnable\":true", json);
        Assert.Contains("\"vmPassword\":\"1234\"", json);
    }

    [Fact]
    public void CoreUserRequest_ShouldSerialize_UnderscoredFieldNames()
    {
        var request = new AddCoreUserRequest
        {
            Extension = "101",
            NoAnswerCid = "",
            BusyCid = "",
            ChanUnavailCid = "",
            NoAnswerDestination = "ext-local,101,dest",
            BusyDestination = "ext-local,101,busy",
            ChanUnavailDestination = "ext-local,101,unavail"
        };

        var json = JsonSerializer.Serialize(request, Options);

        Assert.Contains("\"noanswer_cid\"", json);
        Assert.Contains("\"chanunavail_dest\"", json);
    }

    [Fact]
    public void CdrDto_ShouldDeserialize()
    {
        const string json = """
            {"id":"1","calldate":"2026-04-02 08:41:54","src":"101","dst":"s",
             "disposition":"ANSWERED","duration":21,"billsec":21,"uniqueid":"1775119314.0"}
            """;

        var dto = JsonSerializer.Deserialize<CdrDto>(json, Options)!;

        Assert.Equal("101", dto.Source);
        Assert.Equal("ANSWERED", dto.Disposition);
        Assert.Equal(21, dto.Duration);
    }

    [Fact]
    public void QueueDto_ShouldCaptureAdditionalSettings()
    {
        const string json = """
            {"extension":"1001","name":"Support","strategy":"ringall","timeout":15}
            """;

        var dto = JsonSerializer.Deserialize<QueueDto>(json, Options)!;

        Assert.Equal("1001", dto.Extension);
        Assert.Equal("Support", dto.Name);
        Assert.NotNull(dto.AdditionalSettings);
        Assert.True(dto.AdditionalSettings!.ContainsKey("strategy"));
    }
}
