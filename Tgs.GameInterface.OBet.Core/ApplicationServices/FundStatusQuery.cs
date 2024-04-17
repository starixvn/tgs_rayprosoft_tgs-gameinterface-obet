namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentId,
	string PlayerCode,
	string SerialNo);
