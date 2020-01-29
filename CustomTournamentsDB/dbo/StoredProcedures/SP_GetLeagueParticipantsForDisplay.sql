CREATE PROCEDURE [dbo].[SP_GetLeagueParticipantsForDisplay]
	@TournamentId int

AS
BEGIN

	SELECT TeamName, Victories, Draws, Defeats, Scored, Conceded, Points 
	FROM dbo.LeagueParticipants WHERE TournamentId = @TournamentId
	ORDER BY Points DESC, ScoreDifferential DESC, Scored DESC, Conceded ASC

END
