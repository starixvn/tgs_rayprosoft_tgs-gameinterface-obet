namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
