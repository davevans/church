CREATE TABLE [Account].[User]
(
	[Id]				BIGINT			NOT NULL,
	[PersonId]			BIGINT			NOT NULL,
	[HashedPassword]	VARCHAR(100)	NOT NULL,
	[Salt]				VARCHAR(100)	NOT NULL,
	[Created]			DATETIME		NOT NULL,
	[IsActive]			BIT				NOT NULL DEFAULT(0),

	CONSTRAINT PK_User PRIMARY KEY (Id),
	CONSTRAINT FK_User_Person FOREIGN KEY (PersonId) REFERENCES [Core].[Person](Id)
)
