CREATE PROCEDURE [dbo].[SP_InsertNewGame]
	@Id int = 0 output,
	@TournamentId int,
	@RoundId int

AS
BEGIN
	
	INSERT INTO dbo.Games
		(TournamentId, RoundId)
	VALUES
		(@TournamentId, @RoundId)

	SELECT @Id=SCOPE_IDENTITY();

END