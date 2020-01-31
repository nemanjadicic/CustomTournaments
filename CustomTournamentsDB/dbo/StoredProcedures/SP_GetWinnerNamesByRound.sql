CREATE PROCEDURE [dbo].[SP_GetWinnerNamesByRound]
	@RoundId int

AS
BEGIN

	SELECT TeamName FROM GameParticipants WHERE RoundId = @RoundId AND CupRoundWinner = 1;

END
