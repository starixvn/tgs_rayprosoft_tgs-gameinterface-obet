using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core;

public interface IGameApiClient
{
    public ValueTask CreateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    public ValueTask UpdateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    public ValueTask TransferFundAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default);

    public ValueTask<bool> TransferFundRecordEnquiryAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        CancellationToken cancellationToken = default);

    public ValueTask<BalanceResult> GetUserBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    public ValueTask<UserLoginResult> UserLoginAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default);

    public ValueTask UserLogoutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<TicketResult> TicketAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default);
}