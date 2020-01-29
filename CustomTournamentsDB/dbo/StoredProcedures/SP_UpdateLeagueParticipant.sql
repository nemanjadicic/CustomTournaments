CREATE PROCEDURE [dbo].[SP_UpdateLeagueParticipant]
	@Id int,
	@Victories int,
    @Draws int,
    @Defeats int,
    @Scored int,
    @Conceded int,
    @ScoreDifferential int,
    @Points int

AS
BEGIN

	UPDATE dbo.LeagueParticipants
    SET Victories = @Victories, Draws = @Draws, Defeats = @Defeats, Scored = @Scored, 
    Conceded = @Conceded, ScoreDifferential = @ScoreDifferential, Points = @Points
    WHERE Id = @Id;

END
