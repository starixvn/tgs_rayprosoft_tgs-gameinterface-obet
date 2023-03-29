namespace Tgs.GameInterface.OBet.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode);