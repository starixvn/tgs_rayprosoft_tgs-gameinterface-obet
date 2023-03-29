namespace Tgs.GameInterface.OBet.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode,
    string SerialNo);