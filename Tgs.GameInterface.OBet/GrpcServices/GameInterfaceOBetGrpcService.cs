using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.OBet.Core.ApplicationServices;
using Tgs.GameInterface.OBet.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.GrpcServices
{
    public class GameInterfaceOBetGrpcService : GameInterfaceOBet.GameInterfaceOBetBase
    {
        private readonly IGetService<FundInQuery, FundResult> _fundInService;
        private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;
        private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
        private readonly ILogger<GameInterfaceOBetGrpcService> _logger;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
        private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

        public GameInterfaceOBetGrpcService(
            ILogger<GameInterfaceOBetGrpcService> logger,
            IGetService<FundInQuery, FundResult> fundInService,
            IGetService<FundOutQuery, FundResult> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
            IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerCreateCommand> playerCreateService,
            ICommandService<PlayerLogoutCommand> PlayerLogoutService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
            _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
            _playerLogoutService = PlayerLogoutService ?? throw new ArgumentNullException(nameof(PlayerLogoutService));
        }

        public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
        {
            _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

            return Task.FromResult(new CheckingServiceReply
            {
                MachineName = Environment.MachineName
            });
        }

        public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
        {
            var query = new FundInQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode,
                SerialNo: request.SerialNo,
                Amount: request.Amount);

            var result = await _fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                SerialNo = result.SerialNo
            };
        }

        public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
        {
            var query = new FundOutQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode,
                SerialNo: request.SerialNo,
                Amount: request.Amount);

            var result = await _fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                SerialNo = result.SerialNo
            };
        }

        public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode,
                SerialNo: request.SerialNo);

            var result = await _fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = result
            };
        }

        public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                StartDate: request.StartDate.ToDateTimeOffset(),
                EndDate: request.EndDate.ToDateTimeOffset());

            var results = await _gameRecordService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            var reply = new GameRecordReply();

            if (results.Any())
            {
                foreach (var result in results)
                {
                    reply.Items.Add(new GameRecordInfo
                    {
                        ExternalRoundId = result.ExternalRoundId,
                        PlayerCode = result.PlayerCode,
                        IsClosed = result.IsClosed,
                        IsVoid = result.IsVoid,
                        ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = result.ExternalClosedOn.GetValueOrDefault(DateTimeOffset.UnixEpoch).ToTimestamp(),
                        Bet = result.Bet,
                        Commission = result.Commission,
                        Settle = result.Settle,
                        WinLoss = result.WinLoss,
                        AdditionalData = result.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
        {
            var query = new GameUrlQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode,
                Lang: request.Lang,
                IsMobile: request.IsMobile);

            var result = await _gameUrlService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new GameUrlReply
            {
                IsPlayerExist = result.IsPlayerExist,
                GameUrl = result.GameUrl
            };
        }

        public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode);

            var result = await _playerBalanceService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new PlayerBalanceReply
            {
                IsPlayerExist = result.IsPlayerExist,
                Balance = result.Balance
            };
        }

        public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode);

            await _playerCreateService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }

        public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
        {
            var command = new PlayerLogoutCommand(
                ApiUrl: new(request.ApiUrl),
                ApiKey: request.ApiKey,
                AgentId: request.AgentId,
                PlayerCode: request.PlayerCode);

            await _playerLogoutService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}