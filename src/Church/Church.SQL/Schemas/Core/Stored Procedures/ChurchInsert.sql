CREATE PROCEDURE [Core].[ChurchInsert]
	@Name NVARCHAR(500),
	@TimeZoneId INT,
	@Id INT OUTPUT
AS
	

	INSERT INTO [Core].Church (Name, TimeZoneId)
	VALUES (@Name, @TimeZoneId)

	SET @Id = SCOPE_IDENTITY()
