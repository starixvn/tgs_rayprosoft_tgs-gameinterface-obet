namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record PlayerUpdateBetLimitCommand(
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
