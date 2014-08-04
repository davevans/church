CREATE TABLE [Core].[Person]
(
	[Id]					BIGINT IDENTITY(1,1)			NOT NULL,
	[ChurchId]				INT								NOT NULL,
	[FirstName]				NVARCHAR(100)					NOT NULL,
	[MiddleName]			NVARCHAR(100)					NULL,
	[LastName]				NVARCHAR(100)					NOT NULL,
	[DateOfBirthDay]		SMALLINT						NULL,
	[DateOfBirthMonth]		SMALLINT						NULL,
	[DateOfBirthYear]		SMALLINT						NULL,
	[IsMale]				BIT								NOT NULL,
	[Occupation]			NVARCHAR(255)					NULL,
	[TimeZoneId]			INT								NULL,
	[IsArchived]			BIT								NOT NULL DEFAULT(0),
	
	CONSTRAINT PK_Person PRIMARY KEY (Id),
	CONSTRAINT FK_Person_TimeZone FOREIGN KEY (TimeZoneId) REFERENCES [Core].[TimeZone](Id),
	CONSTRAINT FK_Person_Church FOREIGN KEY (ChurchId) REFERENCES [Core].[Church](Id),
)
