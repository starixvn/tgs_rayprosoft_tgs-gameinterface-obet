namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class GameApiUtensilUnitTest
{
    [Fact]
    public void GetUserId()
    {
        var utensil = new GameApiUtensil();

        var actual = utensil.GetUserId("AG0", "test");

        Assert.NotNull(actual);
        Assert.Equal("AG0test", actual);
    }
}
