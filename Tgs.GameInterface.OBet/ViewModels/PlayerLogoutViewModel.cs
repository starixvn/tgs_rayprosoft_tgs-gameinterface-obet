namespace Tgs.GameInterface.OBet.ViewModels;

public record PlayerLogoutViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode);