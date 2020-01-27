CREATE PROCEDURE [dbo].[SP_GetGameParticipantsByGame]
	@GameId int

AS
BEGIN

	SELECT * From GameParticipants WHERE GameId = @GameId

END
