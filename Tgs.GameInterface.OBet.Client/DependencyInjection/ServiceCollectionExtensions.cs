using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Options;
using Tgs.GameInterface.OBet.Client;
using Tgs.GameInterface.OBet.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceOBetClient(
        this IServiceCollection services,
        Action<GameInterfaceOBetClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceOBetClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceOBet.GameInterfaceOBetClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceOBetClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
                configureChannel.Credentials = ChannelCredentials.Insecure;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            //負載平衡時，效能很重要。 藉由快取位址，解析位址的延遲會從 gRPC 呼叫中排除。 進行第一個 gRPC 呼叫時，將會叫用解析程式，而後續的呼叫會使用快取。
            .AddSingleton<ResolverFactory>(sp => new DnsResolverFactory(refreshInterval: TimeSpan.FromSeconds(30)))
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceOBetClient, GameInterfaceOBetClient>();
}
