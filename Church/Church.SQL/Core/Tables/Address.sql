﻿CREATE TABLE [Core].[Address]
(
	[Id]		INT				NOT NULL,
	[Street1]	NVARCHAR(255)	NOT NULL,
	[Street2]	NVARCHAR(255)	NULL,
	[City]		NVARCHAR(255)	NULL,
	[Postcode]	VARCHAR(10)		NULL,
	[State]		NVARCHAR(50)	NULL,
	[Country]	VARCHAR(50)		NULL, 
	CONSTRAINT [PK_Core.Address] PRIMARY KEY CLUSTERED (Id)
)