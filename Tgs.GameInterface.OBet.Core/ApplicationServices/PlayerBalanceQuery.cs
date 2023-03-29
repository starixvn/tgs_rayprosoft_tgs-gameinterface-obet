namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode);
