CREATE PROCEDURE [dbo].[SP_InsertNewPrize]
	@Id int = 0 output,
	@TournamentId int,
	@PlaceNumber int,
	@PlaceName nvarchar(50),
	@PrizeAmount money

AS
BEGIN

	INSERT INTO dbo.Prizes
		(TournamentId, PlaceNumber, PlaceName, PrizeAmount)
	VALUES
		(@TournamentId, @PlaceNumber, @PlaceName, @PrizeAmount)

	SELECT @Id = SCOPE_IDENTITY();

END
