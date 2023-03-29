namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record PlayerBalanceResult(
    bool IsPlayerExist,
    decimal Balance);