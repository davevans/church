CREATE PROCEDURE [Account].[UserInsert]
	@PersonId BIGINT,
	@HashedPassword VARCHAR(100),
	@Salt VARCHAR(100),
	@IsActive BIT,
	@Id BIGINT OUTPUT
AS
	

	INSERT INTO Account.[User] 
	(
		PersonId,
		HashedPassword,
		Salt,
		IsActive
	)
	VALUES
	(
		@PersonId,
		@HashedPassword,
		@Salt,
		@IsActive
	)

	SET @Id = SCOPE_IDENTITY()
