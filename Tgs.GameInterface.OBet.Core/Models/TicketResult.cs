namespace Tgs.GameInterface.OBet.Core.Models;

public record TicketResult(
	string ExternalRoundId,
	string UserId,
	TicketSatus Status,
	TicketWinLossStatus? WinLossStatus,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset? ExternalClosedOn,
	decimal Bet,
	decimal Commission,
	decimal WinLoss,
	string AdditionalData);
