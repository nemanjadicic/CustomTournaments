CREATE PROCEDURE [dbo].[SP_GetLeagueParticipantByName]
	@TeamName nvarchar(100),
	@TournamentId int

AS
BEGIN

	SELECT * FROM LeagueParticipants
	WHERE TeamName = @TeamName AND TournamentId = @TournamentId;

END
