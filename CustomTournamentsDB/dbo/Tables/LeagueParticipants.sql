CREATE TABLE [dbo].[LeagueParticipants]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL, 
    [TeamName] NVARCHAR(100) NOT NULL,
    [Victories] INT NULL, 
    [Draws] INT NULL, 
    [Defeats] INT NULL, 
    [Scored] INT NULL, 
    [Conceded] INT NULL,
    [ScoreDifferential] INT NULL,
    [Points] INT NULL, 
    CONSTRAINT [FK_LeagueParticipants_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id]) 
)
