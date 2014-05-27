CREATE TABLE [Core].[Church] (
    [Id]		INT				NOT NULL, 
	[Name]		VARCHAR(255)	NOT NULL,
    CONSTRAINT [PK_Core.Church_Id] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [UC_Core.Church_Name] UNIQUE ([Name])
);

