namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string ApiKey,
	string AgentId,
	string PlayerCode);
