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
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES124UCMQPLXB029",
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Pending_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES124UCMQPLXB029",
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task Refunded_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES124UCMQPLXB029",
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Rejected_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12CNY",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES12CNY4UGZ7KLO94OA",
            TicketSatus.Rejected,
            null,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            10,
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ugz7klo94oa", actual.First().PlayerCode);
        Assert.True(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
    }

    [Fact]
    public async Task Settlement_Bets_Lose()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES124UCMQPLXB029",
            TicketSatus.Accepted,
            TicketWinLossStatus.Lose,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            10,
            0,
            -10,
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
        Assert.False(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(0m, actual.First().Settle);
        Assert.Equal(-10m, actual.First().WinLoss);
    }

    [Fact]
    public async Task Settlement_Bets_Win()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12CNY",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES12CNY4UGZ7KLO94OA",
            TicketSatus.Accepted,
            TicketWinLossStatus.Win,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            DateTimeOffset.UtcNow,
            10m,
            0,
            9.6m,
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ugz7klo94oa", actual.First().PlayerCode);
        Assert.False(actual.First().IsVoid);
        Assert.True(actual.First().IsClosed);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(19.6m, actual.First().Settle);
        Assert.Equal(9.6m, actual.First().WinLoss);
    }

    [Fact]
    public async Task Unsettled_Bets()
    {
        var fakeLog = NullLogger<GameRecordQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "YYTES12",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var results = new TicketResult[] {
        new(
            Guid.NewGuid().ToString("N"),
            "YYTES124UCMQPLXB029",
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

        var actual = await client.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
        Assert.False(actual.First().IsVoid);
        Assert.False(actual.First().IsClosed);
    }
}
