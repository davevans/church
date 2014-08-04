CREATE PROCEDURE [Core].[ChurchGet]
	@Ids dbo.integer_list_tbltype readonly
AS
	
	SELECT 
		c.Id,
		c.Name,
		c.Created,
		c.LastUpdated,
		c.IsArchived,
		tz.Id As TimeZoneId,
		tz.Name As TimeZoneName,
		loc.Id As LocationId,
		loc.Name As LocationName,
		a.Id As AddressId,
		a.City As AddressCity,
		a.Country As AddressCountry,
		a.Postcode as AddressPostcode,
		a.[State] AS AddressState,
		a.Street1 As AddressStreet1,
		a.Street2 As AddressStreet2

	FROM [Core].Church c JOIN @Ids ids
		ON c.Id = ids.Id
	JOIN [Core].TimeZone tz
		ON tz.Id = c.TimeZoneId
	LEFT JOIN [Core].Location loc
		ON loc.ChurchId = c.Id
	LEFT JOIN [Core].[Address] a
		ON a.Id = loc.AddressId
	
	
		
