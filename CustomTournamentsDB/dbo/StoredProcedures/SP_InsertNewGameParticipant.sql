CREATE PROCEDURE [dbo].[SP_InsertNewGameParticipant]
	@Id int = 0 output,
	@RoundId int,
	@GameId int,
	@TeamName nvarchar(100)

AS
BEGIN

	INSERT INTO dbo.GameParticipants
		(RoundId, GameId, TeamName)
	VALUES
		(@RoundId, @GameId, @TeamName)

	SELECT @Id = SCOPE_IDENTITY();

END
