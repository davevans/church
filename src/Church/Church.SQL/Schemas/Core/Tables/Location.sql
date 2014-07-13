CREATE TABLE [Core].[Location]
(
	[Id]		INT	IDENTITY(1,1)	NOT NULL,
	[Name]		NVARCHAR(255)		NOT NULL,
	[ChurchId]	INT					NOT NULL,
	[AddressId]	INT					NOT NULL,
	CONSTRAINT PK_Location	PRIMARY KEY (Id),
	CONSTRAINT FK_Location_ChurchId FOREIGN KEY (ChurchId) REFERENCES [Core].Church(Id),
	CONSTRAINT FK_Location_AddressId FOREIGN KEY (AddressId) REFERENCES [Core].[Address](Id)
)
