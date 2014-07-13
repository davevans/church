IF NOT EXISTS (SELECT 1 FROM Core.[Address] a WHERE a.Id = 1)
BEGIN
	INSERT INTO [Core].[Address] (Street1, Street2, City, Postcode, Country)
	VALUES
	(
		'55 Erskineville Rd',
		'Erskineville',
		'Sydney',
		'2043',
		'Australia'
	)
END

IF NOT EXISTS (SELECT 1 FROM Core.[Address] a WHERE a.Id = 2)
BEGIN
	INSERT INTO [Core].[Address] (Street1, Street2, City, Postcode, Country)
	VALUES
	(
		'189 Church St',
		'Newtown',
		'Sydney',
		'2042',
		'Australia'
	)
END