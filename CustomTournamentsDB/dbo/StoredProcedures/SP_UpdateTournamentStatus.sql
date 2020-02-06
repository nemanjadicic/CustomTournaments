CREATE PROCEDURE [dbo].[SP_UpdateTournamentStatus]
	@TournamentId int

AS
BEGIN

	UPDATE Tournaments
	SET Finished = 1 WHERE Id = @TournamentId;

END
