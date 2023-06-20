using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.OBet.Core.ApplicationServices;
using Tgs.GameInterface.OBet.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Controllers
{
    [Route("obet")]
    [ApiController]
    public class OBetController : ControllerBase
    {
        /// <summary>
        /// Funds the in asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029",
        ///             "serialNo":"696FDA6D74E14D6998CDDA11E68B2C3C",
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/in", Name = "FundIn")]
        public async ValueTask<FundResultViewModel> FundInAsync(
            [FromServices] IGetService<FundInQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundInQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode,
                SerialNo: source.SerialNo,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(
                SerialNo: result.SerialNo);
        }

        /// <summary>
        /// Funds the out asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029",
        ///             "serialNo":"5331E638E11245C3B321170FEBE378E8",
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/out", Name = "FundOut")]
        public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundOutQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode,
                SerialNo: source.SerialNo,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(
                SerialNo: result.SerialNo);
        }

        /// <summary>
        /// Funds the status asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029",
        ///             "serialNo":"5331E638E11245C3B321170FEBE378E8"
        ///         }
        /// </remarks>
        [HttpPost("fund/Status", Name = "FundStatus")]
        public ValueTask<bool> FundStatusAsync(
            [FromServices] IGetService<FundStatusQuery, bool> service,
            FundStatusViewModel source)
        {
            var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode,
                SerialNo: source.SerialNo);

            return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Games the record asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "startDate": "2023-03-13T15:45:00+08:00",
        ///             "endDate": "2023-03-13T16:00:00+08:00"
        ///         }
        /// </remarks>
        [HttpPost("game/record")]
        public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordAsync(
            [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
            GameRecordViewModel source)
        {
            var query = new GameRecordQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                StartDate: source.StartDate,
                EndDate: source.EndDate);

            var results = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            if (!results.Any())
            {
                yield break;
            }

            foreach (var result in results)
            {
                yield return new GameRecordResultViewModel(
                    ExternalRoundId: result.ExternalRoundId,
                    PlayerCode: result.PlayerCode,
                    IsClosed: result.IsClosed,
                    IsVoid: result.IsVoid,
                    ExternalCreatedOn: result.ExternalCreatedOn,
                    ExternalClosedOn: result.ExternalClosedOn,
                    Bet: result.Bet,
                    Commission: result.Commission,
                    Settle: result.Settle,
                    WinLoss: result.WinLoss,
                    AdditionalData: result.AdditionalData);
            }
        }

        /// <summary>
        /// Games the URL asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029",
        ///             "lang": "zh-cn",
        ///             "isMobile": false
        ///         }
        /// </remarks>
        [HttpPost("gameurl")]
        public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
            [FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
            GameUrlQueryViewModel source)
        {
            var query = new GameUrlQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode,
                Lang: source.Lang,
                IsMobile: source.IsMobile);

            var result = await service
             .GetAsync(
                 query: query,
                 cancellationToken: HttpContext.RequestAborted)
             .ConfigureAwait(false);

            return new GameUrlResultViewModel(
                result.IsPlayerExist,
                result.GameUrl);
        }

        /// <summary>
        /// Players the balance asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/balance", Name = "PlayerBalance")]
        public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
            [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
            PlayerBalanceViewModel source)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode);

            var result = await service
                    .GetAsync(
                        query: query,
                        cancellationToken: HttpContext.RequestAborted)
                    .ConfigureAwait(false);

            return new PlayerBalanceResultViewModel(
                result.IsPlayerExist,
                result.Balance);
        }

        /// <summary>
        /// Players the create asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/create", Name = "PlayerCreate")]
        public ValueTask PlayerCreateAsync(
            [FromServices] ICommandService<PlayerCreateCommand> service,
            PlayerCreateViewModel source)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode);
            return service.ExecuteAsync(
                    command: command,
                    cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Players the kick asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/logout", Name = "PlayerLogout")]
        public ValueTask PlayerKickAsync(
            [FromServices] ICommandService<PlayerLogoutCommand> service,
            PlayerLogoutViewModel source)
            => service
                .ExecuteAsync(
                    command: new PlayerLogoutCommand(
                        ApiUrl: source.ApiUrl,
                        ApiKey: source.ApiKey,
                        AgentId: source.AgentId,
                        PlayerCode: source.PlayerCode),
                    cancellationToken: HttpContext.RequestAborted);

        /// <summary>
        /// Players the create asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://sports-api.ob33club.com/",
        ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
        ///             "agentId": "YYTES12",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/update/bet-limi", Name = "PlayerCreate")]
        public ValueTask PlayerUpdateBetLimitAsync(
            [FromServices] ICommandService<PlayerUpdateBetLimitCommand> service,
            PlayerUpdateBetLimitViewModel source)
        {
            var command = new PlayerUpdateBetLimitCommand(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentId: source.AgentId,
                PlayerCode: source.PlayerCode,
                MaxBetGrpHdp: source.MaxBetGrpHdp,
                MaxBetGrpBasketBall: source.MaxBetGrpBasketBall,
                MaxBetGrpOthersSport: source.MaxBetGrpOthersSport,
                MaxBetGrp1x2: source.MaxBetGrp1x2,
                MaxBetGrpMixParlay: source.MaxBetGrpMixParlay,
                MaxBetGrpOthersOdds: source.MaxBetGrpOthersOdds,
                MaxBetGrpSpecial: source.MaxBetGrpSpecial);
            return service.ExecuteAsync(
                    command: command,
                    cancellationToken: HttpContext.RequestAborted);
        }
    }
}
