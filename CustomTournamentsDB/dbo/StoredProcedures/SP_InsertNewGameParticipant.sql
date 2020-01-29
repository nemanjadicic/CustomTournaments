CREATE PROCEDURE [dbo].[SP_InsertNewGameParticipant]
	@Id int = 0 output,
	@TournamentId int,
	@RoundId int,
	@GameId int,
	@TeamName nvarchar(100)

AS
BEGIN

	INSERT INTO dbo.GameParticipants
		(TournamentId, RoundId, GameId, TeamName)
	VALUES
		(@TournamentId, @RoundId, @GameId, @TeamName)

	SELECT @Id = SCOPE_IDENTITY();

END
