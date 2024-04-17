using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core;

public interface IGameApiClient
{
	public ValueTask CreateAccountAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		CancellationToken cancellationToken = default);

	public ValueTask<BalanceResult> GetUserBalanceAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		CancellationToken cancellationToken = default);

	public IAsyncEnumerable<TicketResult> TicketAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		DateTimeOffset startDate,
		DateTimeOffset endDate,
		CancellationToken cancellationToken = default);

	public ValueTask TransferFundAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		string serialNo,
		decimal amount,
		CancellationToken cancellationToken = default);

	public ValueTask<bool> TransferFundRecordEnquiryAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		string serialNo,
		CancellationToken cancellationToken = default);

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
	public ValueTask UpdateAccountAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		decimal maxBetGrpHdp,
		decimal maxBetGrpBasketBall,
		decimal maxBetGrpOthersSport,
		decimal maxBetGrp1x2,
		decimal maxBetGrpMixParlay,
		decimal maxBetGrpOthersOdds,
		decimal maxBetGrpSpecial,
		CancellationToken cancellationToken = default);

	public ValueTask<UserLoginResult> UserLoginAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		string lang,
		bool isMobile,
		CancellationToken cancellationToken = default);

	public ValueTask UserLogoutAsync(
		Uri apiUrl,
		string apiKey,
		string agentId,
		string playerCode,
		CancellationToken cancellationToken = default);
}
