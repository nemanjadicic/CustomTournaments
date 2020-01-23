CREATE PROCEDURE [dbo].[SP_InsertNewTeam]
	@Id int = 0 output,
	@TeamName nvarchar(100)

AS
BEGIN

	INSERT INTO dbo.Teams
		(TeamName)
	VALUES
		(@TeamName)

	SELECT @Id = SCOPE_IDENTITY();

END
