namespace Tgs.GameInterface.OBet.ViewModels;

public record PlayerCreateViewModel(
	Uri ApiUrl,
	string ApiKey,
	string AgentId,
	string PlayerCode);
