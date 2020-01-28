CREATE PROCEDURE [dbo].[SP_UpdateGameStatus]
	@Id int,
	@Unplayed bit

AS
BEGIN

	UPDATE dbo.Games
	SET Unplayed = @Unplayed WHERE Id = @Id;

END
