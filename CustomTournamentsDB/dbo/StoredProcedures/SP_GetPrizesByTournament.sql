CREATE PROCEDURE [dbo].[SP_GetPrizesByTournament]
	@TournamentId int

AS
BEGIN

	SELECT * FROM dbo.Prizes WHERE TournamentId = @TournamentId;

END
