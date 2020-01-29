CREATE PROCEDURE [dbo].[SP_GetLeagueParticipantsByTournament]
	@TournamentId int

AS
BEGIN

	SELECT * FROM dbo.LeagueParticipants WHERE TournamentId = @TournamentId;

END
