CREATE TABLE [dbo].[Tournaments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentName] NVARCHAR(100) NOT NULL, 
    [IsLeague] BIT NOT NULL, 
    [EntryFee] MONEY NULL
)
