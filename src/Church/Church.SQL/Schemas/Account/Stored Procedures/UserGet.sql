CREATE PROCEDURE [Account].[UserGet]
	@Ids dbo.big_integer_list_tbltype readonly
AS
	
	SELECT 
		u.Id,
		u.IsActive,
		u.HashedPassword,
		u.PersonId,
		u.Salt,
		u.Created
	FROM
		Account.[User] u JOIN @Ids i
			ON u.Id = i.Id
	
		


