CREATE PROCEDURE [dbo].[SP_InsertNewRound]
	@Id int = 0 output,
	@TournamentId int,
	@RoundNumber int

AS
BEGIN

	INSERT INTO dbo.Rounds
		(TournamentId, RoundNumber)
	VALUES
		(@TournamentId, @RoundNumber)

	SELECT @Id = SCOPE_IDENTITY();

END
