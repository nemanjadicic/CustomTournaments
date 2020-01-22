CREATE TABLE [dbo].[Games]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL,
    [RoundId] INT NOT NULL, 
    CONSTRAINT [FK_Games_RoundId] FOREIGN KEY ([RoundId]) REFERENCES [Rounds]([Id]), 
    CONSTRAINT [FK_Games_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id])
)
