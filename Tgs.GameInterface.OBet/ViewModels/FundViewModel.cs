namespace Tgs.GameInterface.OBet.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode,
    string SerialNo,
    decimal Amount);