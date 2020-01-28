CREATE PROCEDURE [dbo].[SP_UpdateGameParticipantScore]
	@Id int,
	@Score int

AS
BEGIN

	UPDATE GameParticipants
	SET Score = @Score WHERE Id = @Id;

END
