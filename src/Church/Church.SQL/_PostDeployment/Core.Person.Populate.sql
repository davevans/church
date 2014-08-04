IF NOT EXISTS(SELECT 1 FROM [Core].Person p WHERE p.Id = 1) 
BEGIN
	INSERT INTO [Core].Person
	(
		[ChurchId],			
		[FirstName],			
		[MiddleName],		
		[LastName]	,		
		[DateOfBirthDay],	
		[DateOfBirthMonth],	
		[DateOfBirthYear],	
		[IsMale],			
		[Occupation],		
		[TimeZoneId],		
		[IsArchived]		
	)
	VALUES
	(
		1,
		'Dav',
		'Owen',
		'Evans',
		12,
		4,
		1981,
		1,
		'Developer',
		85,
		0
	)
END