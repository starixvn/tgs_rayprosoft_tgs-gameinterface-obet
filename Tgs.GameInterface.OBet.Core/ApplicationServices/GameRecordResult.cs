namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record GameRecordResult(
    string ExternalRoundId,
    string PlayerCode,
    bool IsClosed,
    bool IsVoid,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset? ExternalClosedOn,
    decimal Bet,
    decimal Commission,
    decimal Settle,
    decimal WinLoss,
    string AdditionalData);
