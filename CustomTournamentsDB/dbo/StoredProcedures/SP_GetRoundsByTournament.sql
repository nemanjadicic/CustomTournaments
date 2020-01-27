CREATE PROCEDURE [dbo].[SP_GetRoundsByTournament]
	@TournamentId int

AS
BEGIN

	SELECT * FROM Rounds WHERE TournamentId = @TournamentId

END
