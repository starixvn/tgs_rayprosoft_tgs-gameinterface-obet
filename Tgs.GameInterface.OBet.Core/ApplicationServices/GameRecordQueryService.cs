using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.OBet.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class GameRecordQueryService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly ILogger<GameRecordQueryService> _logger;
    private readonly IGameApiClient _client;

    public GameRecordQueryService(
        ILogger<GameRecordQueryService> logger,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(
        GameRecordQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var results = await _client
            .TicketAsync(
                apiUrl: query.ApiUrl,
                apiKey: query.ApiKey,
                agentId: query.AgentId,
                startDate: query.StartDate,
                endDate: query.EndDate,
                cancellationToken: cancellationToken)
            .ToArrayAsync(
                cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<TicketResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            if (result.Status == TicketSatus.Pending)
            {
                _logger.LogWarning("The game record status is pending.({result})", result);
                continue;
            }

            var isClosed = result.Status switch
            {
                TicketSatus.Rejected => true,
                TicketSatus.Cancelled => true,
                TicketSatus.Refunded => true,
                _ => result.WinLossStatus is not null
            };


            var isVoid = result.Status != TicketSatus.Accepted;


            yield return new GameRecordResult(
                ExternalRoundId: result.ExternalRoundId,
                PlayerCode: result.UserId.Replace(query.AgentId, string.Empty),
                IsClosed: isClosed,
                IsVoid: isVoid,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                Commission: result.Commission,
                Settle: result.WinLoss + result.Bet,
                WinLoss: result.WinLoss,
                AdditionalData: result.AdditionalData);
        }
    }
}