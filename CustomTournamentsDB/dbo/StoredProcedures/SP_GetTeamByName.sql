CREATE PROCEDURE [dbo].[SP_GetTeamByName]
	@TeamName nvarchar(100)
AS
BEGIN

	SELECT * FROM Teams
	WHERE TeamName = @TeamName;

END
