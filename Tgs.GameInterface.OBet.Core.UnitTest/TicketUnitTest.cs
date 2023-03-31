using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.OBet.Core.Extensions;
using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class TicketUnitTest
{
    [Fact]
    public async Task Cancelled_Bets()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id = "OU0792967714",
                    SDate = "2020-11-30T00:00:00",
                    TDate = "2020-11-30T20:06:42.77",
                    kDate = "2020-12-01T01:30:00",
                    ADate = "2020-11-30T20:06:42.77",
                    PDate = "2020-12-01T02:21:23.203",
                    User = "AG0SSS333",
                    Sport = "Football",
                    League = "English Premier League",
                    Home = "Leicester City",
                    Away = "Fulham",
                    Sport_en = "Football",
                    League_en = "English Premier League",
                    Home_en = "Leicester City",
                    Away_en = "Fulham",
                    Live = "False",
                    HomeScore = null as string,
                    AwayScore = null as string,
                    Status = "4",
                    Ft = "h",
                    OddsType = "MY",
                    Game = "OU",
                    Info = "1",
                    Odds = "0.77",
                    Side = "o",
                    Side_en = null as string,
                    Bamt = "5.0000",
                    Wamt = "3.8500",
                    Tresult = null as string,
                    Fhscore = "0-2",
                    Ftscore = "1-2",
                    Fg = "a",
                    Lg = "h",
                    Ip = "",
                    IsMobile = 0,
                    Commission = "0.0200"
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
    }

    [Fact]
    public async Task Data_Is_Empty()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = Array.Empty<object>(),
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Pending_Bets()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id = "OU0792967714",
                    SDate = "2020-11-30T00:00:00",
                    TDate = "2020-11-30T20:06:42.77",
                    kDate = "2020-12-01T01:30:00",
                    ADate = "2020-11-30T20:06:42.77",
                    PDate =  null as string,
                    User = "AG0SSS333",
                    Sport = "Football",
                    League = "English Premier League",
                    Home = "Leicester City",
                    Away = "Fulham",
                    Sport_en = "Football",
                    League_en = "English Premier League",
                    Home_en = "Leicester City",
                    Away_en = "Fulham",
                    Live = "False",
                    HomeScore = null as string,
                    AwayScore = null as string,
                    Status = "-1",
                    Ft = "h",
                    OddsType = "MY",
                    Game = "OU",
                    Info = "1",
                    Odds = "0.77",
                    Side = "o",
                    Side_en = null as string,
                    Bamt = "5.0000",
                    Wamt = "3.8500",
                    Tresult = null as string,
                    Fhscore = "0-2",
                    Ftscore = "1-2",
                    Fg = "a",
                    Lg = "h",
                    Ip = "",
                    IsMobile = 0,
                    Commission = "0.0200"
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Null(actual.First().ExternalClosedOn);
    }

    [Fact]
    public async Task Refunded_Bets()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id = "OU0792967714",
                    SDate = "2020-11-30T00:00:00",
                    TDate = "2020-11-30T20:06:42.77",
                    kDate = "2020-12-01T01:30:00",
                    ADate = "2020-11-30T20:06:42.77",
                    PDate = "2020-12-01T02:21:23.203",
                    User = "AG0SSS333",
                    Sport = "Football",
                    League = "English Premier League",
                    Home = "Leicester City",
                    Away = "Fulham",
                    Sport_en = "Football",
                    League_en = "English Premier League",
                    Home_en = "Leicester City",
                    Away_en = "Fulham",
                    Live = "False",
                    HomeScore = null as string,
                    AwayScore = null as string,
                    Status = "5",
                    Ft = "h",
                    OddsType = "MY",
                    Game = "OU",
                    Info = "1",
                    Odds = "0.77",
                    Side = "o",
                    Side_en = null as string,
                    Bamt = "5.0000",
                    Wamt = "3.8500",
                    Tresult = null as string,
                    Fhscore = "0-2",
                    Ftscore = "1-2",
                    Fg = "a",
                    Lg = "h",
                    Ip = "",
                    IsMobile = 0,
                    Commission = "0.0200"
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
    }

    [Fact]
    public async Task Rejected_Bets()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id= "HDP1461470372554",
                    SDate= "2023-03-31T00:00:00",
                    TDate= "2023-03-31T14:22:16.77",
                    KDate= "2023-03-31T14:15:00",
                    ADate= "2023-03-31T14:22:39.723",
                    PDate= "2023-03-31T14:22:39.723",
                    User= "YYTES12CNY4UGZ7KLO94OA",
                    Sport= "Football",
                    League= "Saba International Friendly Virtual PES 21 - 20 Mins Play",
                    Home= "Oman (V)",
                    Away= "India (V)",
                    Sport_en= "Football",
                    League_en= "Saba International Friendly Virtual PES 21 - 20 Mins Play",
                    Home_en= "Oman (V)",
                    Away_en= "India (V)",
                    Live= "True",
                    HomeScore= "0",
                    AwayScore= "0",
                    Status= "1",
                    FT= "f",
                    OddsType= "HK",
                    Game= "HDP",
                    Info= "0.5",
                    Odds= "0.75",
                    Side= "h",
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= null as object,
                    Tresult= null as object,
                    Fhscore= "2-1",
                    Ftscore= "4-3",
                    FG= null as object,
                    LG= null as object,
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= null as object
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
    }

    [Fact]
    public async Task Settlement_Bets_Draw()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startDate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var endDate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startDate.DatetimeConvertToString();
        query["enddate"] = endDate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id= "OU1459720030614",
                    SDate= "2023-03-29T00:00:00",
                    TDate= "2023-03-29T17:18:51.82",
                    KDate= "2023-03-29T18:00:00",
                    ADate= "2023-03-29T17:18:51.82",
                    PDate= "2023-03-29T20:00:15.78",
                    User= "YYTES124UCMQPLXB029",
                    Sport= "Football",
                    League= "Japan J2 League",
                    Home= "Jubilo Iwata",
                    Away= "Tochigi SC",
                    Sport_en= "Football",
                    League_en= "Japan J2 League",
                    Home_en= "Jubilo Iwata",
                    Away_en= "Tochigi SC",
                    Live= "False",
                    HomeScore= null as object,
                    AwayScore= null as object,
                    Status= "0",
                    FT= "f",
                    OddsType= "HK",
                    Game= "OU",
                    Info= "2",
                    Odds= "1.09",
                    Side= "o",
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= "0.0000",
                    Tresult= "2",
                    Fhscore= "1-0",
                    Ftscore= "2-0",
                    FG= "h",
                    LG= "h",
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= "0.0000"
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startDate,
                  endDate: endDate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
        Assert.Equal(TicketWinLossStatus.Draw, actual.First().WinLossStatus!.Value);
    }

    [Fact]
    public async Task Settlement_Bets_Lose()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startDate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var endDate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startDate.DatetimeConvertToString();
        query["enddate"] = endDate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id= "OU1459290213398",
                    SDate= "2023-03-29T00:00:00",
                    TDate= "2023-03-29T17:26:43.257",
                    KDate= "2023-03-29T17:00:00",
                    ADate= "2023-03-29T17:27:09.493",
                    PDate= "2023-03-29T17:51:40.717",
                    User= "YYTES124UCMQPLXB029",
                    Sport= "Football",
                    League= "Australia Cup Qualifiers",
                    Home= "Preston Lions",
                    Away= "Oakleigh Cannons",
                    Sport_en= "Football",
                    League_en= "Australia Cup Qualifiers",
                    Home_en= "Preston Lions",
                    Away_en= "Oakleigh Cannons",
                    Live= "True",
                    HomeScore= "0",
                    AwayScore= "1",
                    Status= "0",
                    FT= "h",
                    OddsType= "HK",
                    Game= "OU",
                    Info= "1.75",
                    Odds= "0.93",
                    Side= "u",
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= "-10.0000",
                    Tresult= "0",
                    Fhscore= "1-2",
                    Ftscore= "1-3",
                    FG= "a",
                    LG= "a",
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= "0.0000"
                }
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startDate,
                  endDate: endDate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
        Assert.Equal(TicketWinLossStatus.Lose, actual.First().WinLossStatus!.Value);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(-10m, actual.First().WinLoss);
    }

    [Fact]
    public async Task Settlement_Bets_Win()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startDate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var endDate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startDate.DatetimeConvertToString();
        query["enddate"] = endDate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new {
                    Id= "OU1461890252177",
                    SDate= "2023-03-31T00:00:00",
                    TDate= "2023-03-31T14:23:52.79",
                    KDate= "2023-03-31T14:15:00",
                    ADate= "2023-03-31T14:24:24.72",
                    PDate= "2023-03-31T14:49:05.753",
                    User= "YYTES12CNY4UGZ7KLO94OA",
                    Sport= "Football",
                    League= "Saba International Friendly Virtual PES 21 - 20 Mins Play",
                    Home= "Denmark (V)",
                    Away= "Poland (V)",
                    Sport_en= "Football",
                    League_en= "Saba International Friendly Virtual PES 21 - 20 Mins Play",
                    Home_en= "Denmark (V)",
                    Away_en= "Poland (V)",
                    Live= "True",
                    HomeScore= "0",
                    AwayScore= "0",
                    Status= "0",
                    FT= "f",
                    OddsType= "HK",
                    Game= "OU",
                    Info= "1.75",
                    Odds= "0.96",
                    Side= "o",
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= "9.6000",
                    Tresult= "1",
                    Fhscore= "0-0",
                    Ftscore= "1-2",
                    FG= null as object,
                    LG= null as object,
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= "0.0000" },
            },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                    .WithQueryString(query.ToString())
                    .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startDate,
                  endDate: endDate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.NotNull(actual.First().ExternalClosedOn);
        Assert.Equal(TicketWinLossStatus.Win, actual.First().WinLossStatus!.Value);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(9.6m, actual.First().WinLoss);
    }

    [Fact]
    public async Task Unsettled_Bets()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new{
                    Id= "OU1459720030614",
                    SDate= "2023-03-29T00:00:00",
                    TDate= "2023-03-29T17:18:51.82",
                    KDate= "2023-03-29T18:00:00",
                    ADate= "2023-03-29T17:18:51.82",
                    PDate= "0001-01-01T00:00:00",
                    User= "YYTES124UCMQPLXB029",
                    Sport= "Football",
                    League= "Japan J2 League",
                    Home= "Jubilo Iwata",
                    Away= "Tochigi SC",
                    Sport_en= "Football",
                    League_en= "Japan J2 League",
                    Home_en= "Jubilo Iwata",
                    Away_en= "Tochigi SC",
                    Live= "False",
                    HomeScore= null as object,
                    AwayScore= null as object,
                    Status= "0",
                    FT= "f",
                    OddsType= "HK",
                    Game= "OU",
                    Info= "2",
                    Odds= "1.09",
                    Side= "o",
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= null as object,
                    Tresult= null as object,
                    Fhscore= null as object,
                    Ftscore= null as object,
                    FG= null as object,
                    LG= null as object,
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= null as object}
        },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                        .WithQueryString(query.ToString())
                        .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Null(actual.First().ExternalClosedOn);
    }

    [Fact]
    public async Task Unsettled_Bets_Parlay()
    {
        var apiUrl = new Uri("https://sports-api.ob33club.com/");
        var apiKey = "G4sL7r0kKFI5Mm317w99";
        var agentId = "YYTES12";
        var startdate = DateTimeOffset.UtcNow.AddMinutes(-10);
        var enddate = DateTimeOffset.UtcNow.AddMinutes(-10);

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var hash = "B6D1904E84D3E2AEF21171A632822AAC";
        _ = fakeCryptographyUtensil
            .Md5Encrypt(
            $"k={apiKey}&a={agentId}")
            .Returns(hash);

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startdate.DatetimeConvertToString();
        query["enddate"] = enddate.DatetimeConvertToString();

        var response = new
        {
            Status = "success",
            Data = new[] {
                new{
                    Id= "PARLAY1461129194607",
                    SDate= "2023-03-31T00:00:00",
                    TDate= "2023-03-31T14:21:23.583",
                    KDate= "0001-01-01T00:00:00",
                    ADate= "2023-03-31T14:21:23.583",
                    PDate= "0001-01-01T00:00:00",
                    User= "YYTES12CNY4UGZ7KLO94OA",
                    Sport= null as object,
                    League= null as object,
                    Home= null as object,
                    Away= null as object,
                    Sport_en= null as object,
                    League_en= null as object,
                    Home_en= null as object,
                    Away_en= null as object,
                    Live= "False",
                    HomeScore= null as object,
                    AwayScore= null as object,
                    Status= "0",
                    FT= null as object,
                    OddsType= "EU",
                    Game= "PARLAY",
                    Info= null as object,
                    Odds= "7.20",
                    Side= null as object,
                    Side_en= null as object,
                    Bamt= "10.0000",
                    Wamt= null as object,
                    Tresult= null as object,
                    Fhscore= null as object,
                    Ftscore= null as object,
                    FG= null as object,
                    LG= null as object,
                    IP= "114.33.41.237-TW",
                    IsMobile= 0,
                    Commission= null as object
                }
        },
            Message = string.Empty
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/api/sb/betinfo")
                        .WithQueryString(query.ToString())
                        .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeGameApiUtensil,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
              .TicketAsync(
                  apiUrl: apiUrl,
                  apiKey: apiKey,
                  agentId: agentId,
                  startDate: startdate,
                  endDate: enddate)
              .ToArrayAsync()
              .ConfigureAwait(false)
              ?? Enumerable.Empty<TicketResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Null(actual.First().ExternalClosedOn);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(0m, actual.First().Commission);
        Assert.Equal(0m, actual.First().WinLoss);
    }
}
