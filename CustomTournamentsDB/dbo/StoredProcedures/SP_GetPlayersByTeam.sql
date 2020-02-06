CREATE PROCEDURE [dbo].[SP_GetPlayersByTeam]
	@TeamId int

AS
BEGIN

	SELECT player.* FROM dbo.TeamMembers member
	INNER JOIN dbo.Players player ON member.PlayerId = player.Id
	WHERE member.TeamId = @TeamId

END
