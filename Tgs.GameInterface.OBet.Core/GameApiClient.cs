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

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ICryptographyUtensil _cryptographyUtensil;
    private readonly IGameApiUtensil _gameApiUtensil;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        IGameApiUtensil gameApiUtensil,
        ICryptographyUtensil cryptographyUtensil,
         HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameApiUtensil = gameApiUtensil ?? throw new ArgumentNullException(nameof(gameApiUtensil));
        _cryptographyUtensil = cryptographyUtensil ?? throw new ArgumentNullException(nameof(cryptographyUtensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask CreateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
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
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        var url = new Uri(apiUrl, $"/api/sb/bal?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: " GetUserBalance",
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
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["hash"] = hash;
        query["startdate"] = startDate.DatetimeConvertToString();
        query["enddate"] = endDate.DatetimeConvertToString();
        var url = new Uri(apiUrl, $"/api/sb/betinfo?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: " Ticket",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");
        }

        var datas = JsonNode["Data"]?.AsArray()
            ?? throw new Exception($"The game provider responded with an error.({JsonNode.ToJsonString()})");

        if (!datas.Any())
        {
            yield break;
        }

        foreach (var data in datas)
        {
            if (data is null)
            {
                _logger.LogWarning("This data is empty.({data})", data);
                continue;
            }

            var satus = (TicketSatus)Convert.ToInt32(data["Status"]!.ToString());
            var winLossStatus = (data["Tresult"] is null) ? default(TicketWinLossStatus?) : (TicketWinLossStatus)Convert.ToInt32(data["Tresult"]!.ToString());

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
                    Bet: Convert.ToDecimal(data["Bamt"]!.ToString()),
                    Commission: Convert.ToDecimal(data["Commission"]!.ToString()),
                    WinLoss: Convert.ToDecimal(data["Wamt"]!.ToString()),
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
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
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
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
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
            _logger.LogError("The game provider responded with an error.({jsonNode})", JsonNode.ToJsonString());
            return false;
        }

        return true;
    }

    public async ValueTask UpdateAccountAsync(
        Uri apiUrl,
        string apiKey,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
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
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        query["lang"] = lang;
        query["mobile"] = ((isMobile) ? 1 : 0).ToString();
        var url = new Uri(apiUrl, $"/api/sb/open?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: " UserLogin",
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
        var userId = _gameApiUtensil.GetUserId(agentId, playerCode);
        var hash = _cryptographyUtensil.Md5Encrypt($"k={apiKey}&a={agentId}&u={userId}");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["agentId"] = agentId;
        query["userId"] = userId;
        query["hash"] = hash;
        var url = new Uri(apiUrl, $"/api/sb/close?{query}");

        var (StatusCode, JsonNode) = await GetAsync(
                apiUrl: url,
                logPrefix: " UserLogout",
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

        var httpResponseMessage = await _client.GetAsync(apiUrl, cancellationToken);

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
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

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

        var httpResponseMessage = await _client.PostAsync(url, content, cancellationToken);

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
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return new(httpResponseMessage.StatusCode, JsonNode.Parse(responseMessage)!);
    }
}
