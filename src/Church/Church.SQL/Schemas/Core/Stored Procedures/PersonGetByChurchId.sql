CREATE PROCEDURE [Core].[PersonGetByChurchId]
	@ChurchId INT
AS
	
	declare @ids dbo.big_integer_list_tbltype

	INSERT INTO @ids (Id)
	SELECT
		p.Id
	FROM [Core].Person p
	WHERE
		p.ChurchId = @ChurchId


	EXEC [Core].PersonGet @ids

