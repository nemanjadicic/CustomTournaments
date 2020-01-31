CREATE PROCEDURE [dbo].[SP_UpdateGameParticipantCupRoundWinner]
	@Id int

AS
BEGIN

	UPDATE GameParticipants
	SET CupRoundWinner = 1 WHERE Id = @Id;

END
