CREATE PROCEDURE [dbo].[SP_InsertNewLeagueParticipant]
	@Id int = 0 output,
	@TournamentId int,
	@TeamName nvarchar(100)

AS
BEGIN
	
	INSERT INTO dbo.LeagueParticipants
		(TournamentId, TeamName)
	VALUES
		(@TournamentId, @TeamName)

	SELECT @Id = SCOPE_IDENTITY();

END
