using Tgs.GameInterface.OBet.Protos;

namespace Tgs.GameInterface.OBet.Client;

public interface IGameInterfaceOBetClient
{
    ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);
}