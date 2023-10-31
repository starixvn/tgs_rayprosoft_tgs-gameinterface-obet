using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class TransferFundRecordEnquiryUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var serialNo = Guid.NewGuid().ToString("N");

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var userId = "YYTES124ucmqplxb029";
        _ = fakeGameApiUtensil
            .GetUserId(
                agentId: Arg.Is(agentId),
                playerCode: Arg.Is(playerCode))
            .Returns(userId);

        var hash = "8FC127DC25D8465E378F00E46E4429B7";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}&u={userId}")
            .Returns(hash);

        var parameters = new
        {
            hash,
            agentId,
            userId,
            serialNo
        };
        var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = new
        {
            Status = "success",
            Data = null as object,
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/api/sb/fund/check")
            .WithContent(await requestcontent.ReadAsStringAsync())
            .Respond(statusCode: HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client.TransferFundRecordEnquiryAsync(
            apiUrl: apiUrl,
            apiKey: apiKey,
            agentId: agentId,
            playerCode: playerCode,
            serialNo: serialNo);

        Assert.True(actual);
    }

    [Fact]
    public async Task Order_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var serialNo = Guid.NewGuid().ToString("N");

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var userId = "YYTES124ucmqplxb029";
        _ = fakeGameApiUtensil
            .GetUserId(
                agentId: Arg.Is(agentId),
                playerCode: Arg.Is(playerCode))
            .Returns(userId);

        var hash = "8FC127DC25D8465E378F00E46E4429B7";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}&u={userId}")
            .Returns(hash);

        var parameters = new
        {
            hash,
            agentId,
            userId,
            serialNo
        };
        var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = new
        {
            Status = "fail",
            Data = null as object,
            Message = "-1"
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/api/sb/fund/check")
            .WithContent(await requestcontent.ReadAsStringAsync())
            .Respond(statusCode: HttpStatusCode.BadRequest, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client.TransferFundRecordEnquiryAsync(
            apiUrl: apiUrl,
            apiKey: apiKey,
            agentId: agentId,
            playerCode: playerCode,
            serialNo: serialNo);

        Assert.False(actual);
    }
}
