CREATE PROCEDURE [dbo].[SP_InsertNewPlayer]
	@Id int = 0 output,
	@FirstName nvarchar(100),
	@LastName nvarchar(100)

AS
BEGIN
	
	INSERT INTO dbo.Players
		(FirstName, LastName)
	VALUES
		(@FirstName, @LastName)

	SELECT @Id = SCOPE_IDENTITY();

END
