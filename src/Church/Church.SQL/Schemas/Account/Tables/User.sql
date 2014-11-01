CREATE TABLE [Account].[User]
(
	[Id]				BIGINT	IDENTITY(1,1)			NOT NULL,
	[PersonId]			BIGINT							NOT NULL,
	[HashedPassword]	VARCHAR(100)					NOT NULL,
	[Salt]				VARCHAR(100)					NOT NULL,
	[Created]			DATETIME						NOT NULL DEFAULT GETUTCDATE(),
	[IsActive]			BIT								NOT NULL DEFAULT(1),

	CONSTRAINT PK_User PRIMARY KEY (Id),
	CONSTRAINT FK_User_Person FOREIGN KEY (PersonId) REFERENCES [Core].[Person](Id),
	CONSTRAINT UC_User_PersonId UNIQUE ([PersonId])
)
