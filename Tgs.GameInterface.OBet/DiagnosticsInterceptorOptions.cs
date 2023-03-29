using System.Diagnostics;

namespace Tgs.GameInterface.OBet;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.OBet.DiagnosticsInterceptor");
}
