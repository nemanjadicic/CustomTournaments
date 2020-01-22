CREATE PROCEDURE [dbo].[SP_InsertNewPlayer]
	@Id int = 0 OUTPUT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100)

AS
BEGIN
	
	INSERT INTO dbo.Players
		(FirstName, LastName)
	VALUES
		(@FirstName, @LastName)

	SELECT @Id = SCOPE_IDENTITY();

END
