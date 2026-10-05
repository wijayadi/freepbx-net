using Xunit;

namespace Sengsara.Freepbx.IntegrationTests.Objects;

[Collection("FreePbx")]
public class CoreObjectIntegrationTests
{
    private readonly FreePbxFixture _fixture;

    public CoreObjectIntegrationTests(FreePbxFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllCoreUsers_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var users = await _fixture.Client!.CoreUsers.GetAllAsync();
        Assert.NotNull(users);
    }

    [Fact]
    public async Task GetAllCoreDevices_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var devices = await _fixture.Client!.CoreDevices.GetAllAsync();
        Assert.NotNull(devices);
    }

    [Fact]
    public async Task GetAllRingGroups_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var groups = await _fixture.Client!.RingGroups.GetAllAsync();
        Assert.NotNull(groups);
    }

    [Fact]
    public async Task GetAllInboundRoutes_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var routes = await _fixture.Client!.InboundRoutes.GetAllAsync();
        Assert.NotNull(routes);
    }

    [Fact]
    public async Task GetAllRecordings_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var recordings = await _fixture.Client!.Recordings.GetAllAsync();
        Assert.NotNull(recordings);
    }

    [Fact]
    public async Task GetRecordingFiles_ShouldReturnListAndFilter()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var files = await _fixture.Client!.Recordings.GetFilesAsync();
        var filtered = await _fixture.Client.Recordings.GetFilesAsync("custom");

        Assert.NotNull(files);
        Assert.All(filtered, f => Assert.Contains("custom", f, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllMusicOnHold_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var moh = await _fixture.Client!.MusicOnHold.GetAllAsync();
        Assert.NotNull(moh);
    }

    [Fact]
    public async Task GetVoiceMailAndFollowMe_ForFirstExtension_ShouldSucceed()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var extensions = await _fixture.Client!.Extensions.GetAllAsync();
        var first = extensions.FirstOrDefault();
        if (first is null)
        {
            return;
        }

        var voicemail = await _fixture.Client.VoiceMail.GetAsync(first.ExtensionId);
        var followMe = await _fixture.Client.FollowMe.GetAsync(first.ExtensionId);

        Assert.NotNull(voicemail);
        Assert.NotNull(followMe);
    }

    [Fact]
    public async Task GetCdrs_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var cdrs = await _fixture.Client!.Cdrs.GetAsync(first: 5);
        Assert.NotNull(cdrs);
    }
}
