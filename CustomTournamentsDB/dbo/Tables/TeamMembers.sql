CREATE TABLE [dbo].[TeamMembers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TeamId] INT NOT NULL, 
    [PlayerId] INT NOT NULL, 
    CONSTRAINT [FK_TeamMembers_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]), 
    CONSTRAINT [FK_TeamMembers_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players]([Id])
)
