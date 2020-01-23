CREATE PROCEDURE [dbo].[SP_InsertNewLeagueParticipant]
	@Id int = 0 output,
	@TournamentId int,
	@TeamId int,
	@TeamName nvarchar(100)

AS
BEGIN
	
	INSERT INTO dbo.LeagueParticipants
		(TournamentId, TeamId, TeamName)
	VALUES
		(@TournamentId, @TeamId, @TeamName)

	SELECT @Id = SCOPE_IDENTITY();

END
