namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentId,
	string PlayerCode,
	string Lang,
	bool IsMobile);
