CREATE PROCEDURE [dbo].[SP_InsertNewGame]
	@Id int = 0 output,
	@TournamentId int,
	@RoundId int,
	@Unplayed bit

AS
BEGIN
	
	INSERT INTO dbo.Games
		(TournamentId, RoundId, Unplayed)
	VALUES
		(@TournamentId, @RoundId, @Unplayed)

	SELECT @Id = SCOPE_IDENTITY();

END