CREATE TABLE [dbo].[GameParticipants]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoundId] INT NOT NULL,
    [GameId] INT NOT NULL,
    [PreviousCupGameId] INT NULL,
    [TeamId] INT NULL,
    [TeamName] NVARCHAR(100) NULL,
    [Score] INT NULL,
    [CupRoundWinner] BIT NULL, 
    CONSTRAINT [FK_GameParticipants_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games]([Id]), 
    CONSTRAINT [FK_GameParticipants_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id])
)
