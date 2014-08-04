CREATE PROCEDURE [Core].[ChurchUpdate]
	@Id			INT,
	@Name		NVARCHAR(500),
	@TimeZoneId INT,
	@IsArchived BIT
AS

	UPDATE c
		SET Name = @Name,
			TimeZoneId = @TimeZoneId,
			IsArchived = @IsArchived
	FROM [Core].Church c
	WHERE
	c.Id = @Id
