using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;
using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class GameRecordQueryServiceUnitTest
{
    [Fact]
    public async Task Cancelled_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Cancelled,
            null,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            1000,
            0,
            0,
            "{}" )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Pending_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Pending,
            null,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            null,
            1000,
            0,
            0,
            "{}" )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Refunded_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Refunded,
            null,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            1000,
            0,
            0,
            "{}" )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Rejected_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Rejected,
            null,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            1000,
            0,
            0,
            "{}" )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Settlement_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Accepted,
            TicketWinLossStatus.Win,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            1000,
            0,
            100,
            "{}" )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.False(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
        Assert.Equal(1100m, actual.First().Settle);
    }

    [Fact]
    public async Task Unsettled_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "4ucmqplxb029",
            TicketSatus.Accepted,
            null,
            DateTimeOffset.UtcNow,
            null,
            1000,
            0,
            0,
            "{}"
            )};
        _ = fakeGameApiClient
             .TicketAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.False(actual.First().IsVoid);
        Assert.False(actual.First().IsClosed);
    }
}
