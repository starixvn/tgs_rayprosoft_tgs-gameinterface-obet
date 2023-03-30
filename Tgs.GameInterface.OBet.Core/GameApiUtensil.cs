namespace Tgs.GameInterface.OBet.Core;

internal class GameApiUtensil : IGameApiUtensil
{
    public string GetUserId(string agentId, string playerCode)
        => $"{agentId}{playerCode}";
}
