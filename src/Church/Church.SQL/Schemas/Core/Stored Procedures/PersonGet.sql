CREATE PROCEDURE [Core].[PersonGet]
	@Ids dbo.big_integer_list_tbltype readonly
AS
	
	SELECT
		p.Id,
		p.FirstName,
		p.MiddleName,
		p.LastName,
		p.DateOfBirthDay,
		p.DateOfBirthMonth,
		p.DateOfBirthYear,
		p.ChurchId,
		p.IsMale,
		p.Occupation,
		tz.Id As TimeZoneId,
		tz.Name As TimeZoneName

	FROM [Core].Person p JOIN @Ids i
			On p.Id = i.Id
		 LEFT JOIN [Core].TimeZone tz
			ON tz.Id = p.Id

