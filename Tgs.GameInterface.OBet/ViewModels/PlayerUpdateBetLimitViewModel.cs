namespace Tgs.GameInterface.OBet.ViewModels;

public record PlayerUpdateBetLimitViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    string PlayerCode,
    decimal MaxBetGrpHdp,
    decimal MaxBetGrpBasketBall,
    decimal MaxBetGrpOthersSport,
    decimal MaxBetGrp1x2,
    decimal MaxBetGrpMixParlay,
    decimal MaxBetGrpOthersOdds,
    decimal MaxBetGrpSpecial);
