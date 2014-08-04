CREATE TABLE [Core].[Church]
(
	[Id]			INT					IDENTITY(1,1)	NOT NULL,
	[Name]			NVARCHAR(500)						NOT NULL,
	[TimeZoneId]	INT									NOT NULL,		
	[IsArchived]	BIT									NOT NULL	DEFAULT(0),
	[Created]		DATETIME							NOT NULL	DEFAULT(GETUTCDATE()),
	[LastUpdated]	DATETIME							NOT NULL	DEFAULT(GETUTCDATE())


	CONSTRAINT PK_Church PRIMARY KEY (Id),
	CONSTRAINT FK_Church_TimeZone FOREIGN KEY (TimeZoneId) REFERENCES [Core].[TimeZone](Id),
	CONSTRAINT UC_Church_Name UNIQUE ([Name])

)
