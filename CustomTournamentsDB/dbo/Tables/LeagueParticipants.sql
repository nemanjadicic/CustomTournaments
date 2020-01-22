CREATE TABLE [dbo].[LeagueParticipants]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL, 
    [TeamId] INT NOT NULL, 
    [TeamName] NVARCHAR(100) NOT NULL,
    [Victories] INT NULL DEFAULT 0, 
    [Draws] INT NULL DEFAULT 0, 
    [Defeats] INT NULL DEFAULT 0, 
    [Scored] INT NULL DEFAULT 0, 
    [Conceeded] INT NULL DEFAULT 0, 
    [Points] INT NULL DEFAULT 0, 
    CONSTRAINT [FK_LeagueParticipants_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id]), 
    CONSTRAINT [FK_LeagueParticipants_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id])
)
