CREATE TABLE [dbo].[Tournaments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentName] NVARCHAR(100) NOT NULL, 
    [IsLeague] BIT NOT NULL,
    [HomeAndAway] BIT NULL,
    [VictoryPoints] INT NULL,
    [DrawPoints] INT NULL,
    [OfficialScore] INT NOT NULL,
    [EntryFee] MONEY NULL,
    [Finished] BIT NULL DEFAULT 0
)
