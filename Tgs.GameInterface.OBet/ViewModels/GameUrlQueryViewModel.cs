namespace Tgs.GameInterface.OBet.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode,
    string Lang,
    bool IsMobile);
