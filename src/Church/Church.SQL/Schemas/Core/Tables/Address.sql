﻿CREATE TABLE [Core].[Address]
(
	[Id]			INT	IDENTITY(1,1)	NOT NULL,
	[Street1]		NVARCHAR(255)		NOT NULL,
	[Street2]		NVARCHAR(255)		NULL,
	[City]			NVARCHAR(255)		NULL,
	[State]			NVARCHAR(255)		NULL,
	[Country]		NVARCHAR(255)		NULL,
	[Postcode]		NVARCHAR(20)		NULL,
	CONSTRAINT PK_Address PRIMARY KEY (Id)
)
