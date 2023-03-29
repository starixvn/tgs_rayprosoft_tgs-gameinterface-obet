using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.OBet.Protos;

namespace Tgs.GameInterface.OBet.Client;

internal class GameInterfaceOBetClient : IGameInterfaceOBetClient
{
    private readonly GameInterfaceOBet.GameInterfaceOBetClient _client;

    public GameInterfaceOBetClient(GameInterfaceOBet.GameInterfaceOBetClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode,
                SerialNo = serialNo,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode,
                SerialNo = serialNo,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .FundStatusAsync(
            request: new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode,
                SerialNo = serialNo
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .GameRecordAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                StartDate = startDate.ToTimestamp(),
                EndDate = endDate.ToTimestamp(),
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default)
        => await _client
        .GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode,
                Lang = lang,
                IsMobile = isMobile,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerLogoutAsync(
            request: new PlayerLogoutRequest
            {
                ApiUrl = apiUrl.ToString(),
                ApiKey = apiKey,
                AgentId = agentId,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
