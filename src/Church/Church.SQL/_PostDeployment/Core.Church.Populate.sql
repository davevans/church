IF NOT EXISTS(SELECT 1 FROM [Core].Church c WHERE c.Id = 1) 
BEGIN
	INSERT INTO [Core].Church (Name, TimeZoneId)
	VALUES
	('Erko', 85)
END