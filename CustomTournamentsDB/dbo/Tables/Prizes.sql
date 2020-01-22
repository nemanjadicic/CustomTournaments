CREATE TABLE [dbo].[Prizes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL, 
    [PlaceNumber] INT NOT NULL, 
    [PlaceName] NVARCHAR(50) NOT NULL, 
    [PrizeAmount] MONEY NOT NULL, 
    CONSTRAINT [FK_Prizes_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([Id])
)
