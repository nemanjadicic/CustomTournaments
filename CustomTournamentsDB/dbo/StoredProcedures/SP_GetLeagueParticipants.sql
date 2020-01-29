CREATE PROCEDURE [dbo].[SP_GetLeagueParticipants]
	@TournamentId int

AS
BEGIN

	SELECT TeamName, Victories, Draws, Defeats, Scored, Conceeded, Points 
	FROM dbo.LeagueParticipants WHERE TournamentId = @TournamentId;

END
