CREATE PROCEDURE [dbo].[SP_InsertNewTeamMember]
	@Id int = 0 output,
	@TeamId int,
	@PlayerId int

AS
BEGIN

	INSERT INTO dbo.TeamMembers
		(TeamId, PlayerId)
	VALUES
		(@TeamId, @PlayerId)

	SELECT @Id = SCOPE_IDENTITY();

END
