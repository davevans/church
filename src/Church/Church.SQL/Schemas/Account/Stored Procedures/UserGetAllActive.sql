CREATE PROCEDURE [Account].[UserGetAllActive]

AS

	DECLARE @ids dbo.big_integer_list_tbltype

	INSERT INTO @ids
	SELECT 
		u.Id
	FROM
		Account.[User] u
	WHERE
		u.IsActive = 1


	EXEC Account.UserGet @ids
