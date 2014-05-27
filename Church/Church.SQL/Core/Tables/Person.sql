CREATE TABLE [Core].[Person]
(
	[Id]			INT				NOT NULL,
	[ChurchId]		INT				NOT NULL,
	[FirstName]		NVARCHAR(50)	NOT NULL,
	[MiddleName]	NVARCHAR(50)	NULL,
	[LastName]		NVARCHAR(50)	NOT NULL,
	CONSTRAINT [PK_Core.Person_Id] PRIMARY KEY CLUSTERED (Id), 
    CONSTRAINT [FK_Core.Person_Core.Church] FOREIGN KEY ([ChurchId]) REFERENCES [Core].[Church](Id),
)
