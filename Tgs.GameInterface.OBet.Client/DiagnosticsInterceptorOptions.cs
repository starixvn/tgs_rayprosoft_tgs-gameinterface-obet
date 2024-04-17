using System.Diagnostics;

namespace Tgs.GameInterface.OBet.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.OBet.Client.DiagnosticsInterceptor");
}
