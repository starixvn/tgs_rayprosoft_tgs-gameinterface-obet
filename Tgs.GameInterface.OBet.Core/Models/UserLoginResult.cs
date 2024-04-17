namespace Tgs.GameInterface.OBet.Core.Models;

public record UserLoginResult(
	bool IsPlayerExist,
	string GameUrl);
