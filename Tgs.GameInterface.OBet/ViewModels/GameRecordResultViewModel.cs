namespace Tgs.GameInterface.OBet.ViewModels;

public record GameRecordResultViewModel(
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
