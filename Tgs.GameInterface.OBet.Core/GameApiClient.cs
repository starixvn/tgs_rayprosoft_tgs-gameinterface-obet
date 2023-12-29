using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.OBet.Core.Extensions;
using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core;

internal class GameApiClient(
    ILogger<GameApiClient> logger,
    IGameApiUtensil gameApiUtensil,
    ICryptographyUtensil cryptographyUtensil,
     HttpClient client) : IGameApiClient
{
    public async ValueTask CreateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var parameters = new
        {
            hash,
            agentId,
            userId
        };

        var (StatusCode, JsonNode) = await PostAsync(
                apiUrl: apiUrl,
                path: "/api/sb/register",
                parameters: parameters,
                logPrefix: "CreateNewAccount",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"This player failed to create.({JsonNode.ToJsonString()})");
        }
    }

    public async ValueTask<BalanceResult> GetUserBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        var url = new Uri(apiUrl, $"/api/sb/bal?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: "GetUserBalance",
                cancellationToken: cancellationToken)
            .AsTask()
            .WaitAsync(
                timeout: TimeSpan.FromSeconds(15),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            return StatusCode == HttpStatusCode.NotFound
               ? new BalanceResult(false, 0)
               : throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
        }

        var balance = JsonNode["Data"]!["Balance"]!.GetValue<decimal>();

        return new BalanceResult(true, balance);
    }

    public async IAsyncEnumerable<TicketResult> TicketAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startDate.DatetimeConvertToString();
        query["enddate"] = endDate.DatetimeConvertToString();
        var url = new Uri(apiUrl, $"/api/sb/betinfo?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: "Ticket",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
        }

        var datas = JsonNode["Data"]?.AsArray()
            ?? throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");

        if (datas.Count == 0)
        {
            yield break;
        }

        foreach (var data in datas)
        {
            if (data is null)
            {
                logger.LogWarning("This data is empty.({data})", data);
                continue;
            }

            var satus = (TicketSatus)Convert.ToInt32(data["Status"]!.ToString());
            var winLossStatus = (data["Tresult"] is null) ? default(TicketWinLossStatus?) : (TicketWinLossStatus)Convert.ToInt32(data["Tresult"]!.ToString());
            var bet = (data["Bamt"] is null) ? 0 : Convert.ToDecimal(data["Bamt"]!.ToString());
            var commission = (data["Commission"] is null) ? 0 : Convert.ToDecimal(data["Commission"]!.ToString());
            var wamt = (data["Wamt"] is null) ? 0 : Convert.ToDecimal(data["Wamt"]!.ToString());

            yield return (satus == TicketSatus.Accepted || satus == TicketSatus.Pending) && !winLossStatus.HasValue
                ? new TicketResult(
                    ExternalRoundId: data["Id"]!.GetValue<string>(),
                    UserId: data["User"]!.GetValue<string>(),
                    Status: satus,
                    WinLossStatus: winLossStatus,
                    ExternalCreatedOn: data["TDate"]!.GetValue<string>().StringConvertToDatetime(),
                    ExternalClosedOn: null,
                    Bet: Convert.ToDecimal(data["Bamt"]!.ToString()),
                    Commission: 0,
                    WinLoss: 0,
                    AdditionalData: data.ToJsonString())
                : new TicketResult(
                    ExternalRoundId: data["Id"]!.GetValue<string>(),
                    UserId: data["User"]!.GetValue<string>(),
                    Status: satus,
                    WinLossStatus: winLossStatus,
                    ExternalCreatedOn: data["TDate"]!.GetValue<string>().StringConvertToDatetime(),
                    ExternalClosedOn: data["PDate"]!.GetValue<string>().StringConvertToDatetime(),
                    Bet: bet,
                    Commission: commission,
                    WinLoss: wamt,
                    AdditionalData: data.ToJsonString());
        }
    }

    public async ValueTask TransferFundAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var parameters = new
        {
            hash,
            agentId,
            userId,
            amount,
            serialNo
        };

        var (StatusCode, JsonNode) = await PostAsync(
                apiUrl: apiUrl,
                path: "/api/sb/fund",
                parameters: parameters,
                logPrefix: "TransferFund",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
        }
    }

    public async ValueTask<bool> TransferFundRecordEnquiryAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string serialNo,
        CancellationToken cancellationToken = default)

    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var parameters = new
        {
            hash,
            agentId,
            userId,
            serialNo
        };

        var (StatusCode, JsonNode) = await PostAsync(
                apiUrl: apiUrl,
                path: "/api/sb/fund/check",
                parameters: parameters,
                logPrefix: "TransferFundRecordEnquiry",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            logger.LogError("The game provider responded with an error.({jsonNode})", JsonNode.ToJsonString());
            return false;
        }

        return true;
    }

    /// <summary>
    /// Updates the account asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="apiKey">The API key.</param>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="playerCode">The player code.</param>
    /// <param name="maxBetGrpHdp">The maximum bet GRP HDP.(让球/大小盘/单双)</param>
    /// <param name="maxBetGrpBasketBall">The maximum bet GRP basket ball.(篮球)</param>
    /// <param name="maxBetGrpOthersSport">The maximum bet GRP others sport.(其他运动 (让球,大小盘,单双))</param>
    /// <param name="maxBetGrp1x2">The maximum bet GRP1X2.(独赢/优胜冠军/双重机会)</param>
    /// <param name="maxBetGrpMixParlay">The maximum bet GRP mix parlay.(混合过关)</param>
    /// <param name="maxBetGrpOthersOdds">The maximum bet GRP others odds.(波胆/总进球/半场全场/最先得分/最后得分)</param>
    /// <param name="maxBetGrpSpecial">The maximum bet GRP special.(特别投注)</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async ValueTask UpdateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        decimal maxBetGrpHdp = 0,
        decimal maxBetGrpBasketBall = 0,
        decimal maxBetGrpOthersSport = 0,
        decimal maxBetGrp1x2 = 0,
        decimal maxBetGrpMixParlay = 0,
        decimal maxBetGrpOthersOdds = 0,
        decimal maxBetGrpSpecial = 0,
        CancellationToken cancellationToken = default)
    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");

        var parameters = new JsonObject
        {
            { "hash", hash },
            { "agentId", agentId },
            { "userId", userId }
        };
        if (maxBetGrpHdp > 0)
        {
            parameters.Add("maxbetgrphdp", maxBetGrpHdp);
        }
        if (maxBetGrpBasketBall > 0)
        {
            parameters.Add("maxbetgrpbasketball", maxBetGrpBasketBall);
        }
        if (maxBetGrpOthersSport > 0)
        {
            parameters.Add("maxbetgrpotherssport", maxBetGrpOthersSport);
        }
        if (maxBetGrp1x2 > 0)
        {
            parameters.Add("maxbetgrp1x2", maxBetGrp1x2);
        }
        if (maxBetGrpMixParlay > 0)
        {
            parameters.Add("maxbetgrpmixparlay", maxBetGrpMixParlay);
        }
        if (maxBetGrpOthersOdds > 0)
        {
            parameters.Add("maxbetgrpothersodds", maxBetGrpOthersOdds);
        }
        if (maxBetGrpSpecial > 0)
        {
            parameters.Add("maxbetgrpspecial", maxBetGrpSpecial);
        }
        var (StatusCode, JsonNode) = await PostAsync(
                apiUrl: apiUrl,
                path: "/api/sb/register",
                parameters: parameters,
                logPrefix: "UpdateAccount",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"This player modification failed({JsonNode.ToJsonString()})");
        }
    }

    public async ValueTask<UserLoginResult> UserLoginAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default)
    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        query["lang"] = lang;
        query["mobile"] = ((isMobile) ? 1 : 0).ToString();
        var url = new Uri(apiUrl, $"/api/sb/open?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: "UserLogin",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            return StatusCode == HttpStatusCode.NotFound
               ? new UserLoginResult(false, string.Empty)
               : throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
        }

        var loginUrl = JsonNode["Data"]!["LoginUrl"]!.GetValue<string>()
            ?? throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");

        return new UserLoginResult(true, loginUrl);
    }

    public async ValueTask UserLogoutAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)

    {
        var userId = gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        var url = new Uri(apiUrl, $"/api/sb/close?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: "UserLogout",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            if (StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
            }
        }
    }

    private async ValueTask<(HttpStatusCode StatusCode, JsonNode JsonNode)> GetAsync(
        Uri apiUrl,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var httpResponseMessage = await client.GetAsync(apiUrl, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return new(httpResponseMessage.StatusCode, JsonNode.Parse(responseMessage)!);
    }

    private async ValueTask<(HttpStatusCode StatusCode, JsonNode JsonNode)> PostAsync(
        Uri apiUrl,
        string path,
        dynamic parameters,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var url = new Uri(apiUrl, path);

        HttpContent content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        var httpResponseMessage = await client.PostAsync(url, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return new(httpResponseMessage.StatusCode, JsonNode.Parse(responseMessage)!);
    }
}
