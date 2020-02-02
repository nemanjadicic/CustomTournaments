CREATE PROCEDURE [dbo].[SP_InsertNewTournament]
	@Id int = 0 output,
	@TournamentName nvarchar(100),
	@IsLeague bit,
	@HomeAndAway bit,
	@VictoryPoints int,
	@DrawPoints int,
	@OfficialScore int,
	@EntryFee money
AS
BEGIN

	INSERT INTO dbo.Tournaments
		(TournamentName, IsLeague, HomeAndAway, VictoryPoints, DrawPoints, OfficialScore, EntryFee)
	VALUES
		(@TournamentName, @IsLeague, @HomeAndAway, @VictoryPoints, @DrawPoints, @OfficialScore, @EntryFee)

	SELECT @Id = SCOPE_IDENTITY();

END
