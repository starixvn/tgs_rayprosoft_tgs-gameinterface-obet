namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

public record GameUrlResult(
    bool IsPlayerExist,
    string GameUrl);