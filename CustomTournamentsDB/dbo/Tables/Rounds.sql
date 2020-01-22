CREATE TABLE [dbo].[Rounds]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL,
    [RoundNumber] INT NOT NULL,
    CONSTRAINT [FK_Rounds_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id])
)
