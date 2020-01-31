CREATE TABLE [dbo].[GameParticipants]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [TournamentId] INT NOT NULL,
    [RoundId] INT NOT NULL,
    [GameId] INT NOT NULL,
    [TeamName] NVARCHAR(100) NULL,
    [Score] INT NULL,
    [CupRoundWinner] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_GameParticipants_RoundId] FOREIGN KEY ([RoundId]) REFERENCES [Rounds]([Id]),
    CONSTRAINT [FK_GameParticipants_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games]([Id]), 
    CONSTRAINT [FK_GameParticipants_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id])
)
