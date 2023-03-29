namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class CryptographyUtensilUnitTest
{
    [Fact]
    public void Md5Encrypt()
    {
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";

        var utensil = new CryptographyUtensil();

        var actual = utensil.Md5Encrypt($"k={apiKey}&a={agentId}");

        Assert.NotNull(actual);
        Assert.Equal("B6D1904E84D3E2AEF21171A632822AAC", actual);
    }

    [Fact]
    public void UserId_Md5Encrypt()
    {
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var userId = "YYTES124ucmqplxb029";

        var utensil = new CryptographyUtensil();

        var actual = utensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");

        Assert.NotNull(actual);
        Assert.Equal("8FC127DC25D8465E378F00E46E4429B7", actual);
    }
}