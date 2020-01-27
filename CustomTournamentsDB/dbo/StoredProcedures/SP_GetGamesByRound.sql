CREATE PROCEDURE [dbo].[SP_GetGamesByRound]
	@RoundId int

AS
BEGIN

	SELECT * FROM Games WHERE RoundId = @RoundId

END
