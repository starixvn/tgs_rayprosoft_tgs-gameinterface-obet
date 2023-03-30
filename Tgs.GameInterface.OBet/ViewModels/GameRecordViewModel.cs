namespace Tgs.GameInterface.OBet.ViewModels;

public record GameRecordViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
