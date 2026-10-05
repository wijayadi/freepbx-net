using Xunit;

namespace Sengsara.Freepbx.IntegrationTests.Queues;

[Collection("FreePbx")]
public class QueueIntegrationTests
{
    private readonly FreePbxFixture _fixture;

    public QueueIntegrationTests(FreePbxFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllQueues_ShouldReturnList()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var queues = await _fixture.Client!.Queues.GetAllAsync();

        Assert.NotNull(queues);
        Assert.All(queues, q => Assert.False(string.IsNullOrWhiteSpace(q.Extension)));
    }

    [Fact]
    public async Task GetQueueMembers_ForFirstQueue_ShouldReturnMembers()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var queues = await _fixture.Client!.Queues.GetAllAsync();
        var first = queues.FirstOrDefault();
        if (first is null)
        {
            return;
        }

        var members = await _fixture.Client.Queues.GetMembersAsync(first.Extension);
        var queue = await _fixture.Client.Queues.GetByIdAsync(first.Extension);

        Assert.NotNull(members);
        Assert.NotNull(queue);
    }

    [Fact]
    public async Task GetAllMembers_ShouldReturnDictionary()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        var members = await _fixture.Client!.Queues.GetAllMembersAsync();

        Assert.NotNull(members);
        Assert.All(members.Values, m => Assert.NotNull(m));
    }
}
