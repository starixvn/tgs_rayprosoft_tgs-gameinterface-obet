namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode);
