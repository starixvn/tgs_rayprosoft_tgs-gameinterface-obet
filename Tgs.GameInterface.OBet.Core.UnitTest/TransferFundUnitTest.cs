using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class TransferFundUnitTest
{
    [Fact]
    public async Task Deposit()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var amount = 1.0m;
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
            amount,
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
            .Expect(HttpMethod.Post, "/api/sb/fund")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(statusCode: HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        await client
            .TransferFundAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentId: agentId,
                playerCode: playerCode,
                serialNo: serialNo,
                amount: amount)
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Insufficient_Withdrawal_Balance()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var amount = -1.0m;
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
            amount,
            serialNo
        };
        var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = new
        {
            Status = "fail",
            Data = null as object,
            Message = "101"
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/api/sb/fund")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(statusCode: HttpStatusCode.BadRequest, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        _ = await Assert
            .ThrowsAnyAsync<Exception>(() => client
                .TransferFundAsync(
                    apiUrl: apiUrl,
                    apiKey: apiKey,
                    agentId: agentId,
                    playerCode: playerCode,
                    serialNo: serialNo,
                    amount: amount)
                .AsTask())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var amount = 1.0m;
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
            amount,
            serialNo
        };
        var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = new
        {
            Status = "fail",
            Data = null as object,
            Message = "NotFound"
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/api/sb/fund")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(statusCode: HttpStatusCode.NotFound, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        _ = await Assert
            .ThrowsAnyAsync<Exception>(() => client
                .TransferFundAsync(
                    apiUrl: apiUrl,
                    apiKey: apiKey,
                    agentId: agentId,
                    playerCode: playerCode,
                    serialNo: serialNo,
                    amount: amount)
                .AsTask())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Withdrawal()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var playerCode = "4ucmqplxb029";
        var amount = -1.0m;
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
            amount,
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
            .Expect(HttpMethod.Post, "/api/sb/fund")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(statusCode: HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        await client
            .TransferFundAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentId: agentId,
                playerCode: playerCode,
                serialNo: serialNo,
                amount: amount)
            .ConfigureAwait(false);
    }
}
