namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode,
    string SerialNo,
    decimal Amount);