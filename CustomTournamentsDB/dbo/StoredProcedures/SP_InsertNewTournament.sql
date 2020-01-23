CREATE PROCEDURE [dbo].[SP_InsertNewTournament]
	@Id int = 0 output,
	@TournamentName nvarchar(100),
	@IsLeague bit,
	@EntryFee money
AS
BEGIN

	INSERT INTO dbo.Tournaments
		(TournamentName, IsLeague, EntryFee)
	VALUES
		(@TournamentName, @IsLeague, @EntryFee)

	SELECT @Id = SCOPE_IDENTITY();

END
