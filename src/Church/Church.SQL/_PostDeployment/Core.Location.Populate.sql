IF NOT EXISTS (SELECT 1 FROM Core.Location WHERE Id = 1)
BEGIN
	INSERT INTO Core.[Location] (Name, ChurchId, AddressId)
	VALUES
	('55 Erskineville Rd', 1,1)
END

IF NOT EXISTS (SELECT 1 FROM Core.Location WHERE Id = 2)
BEGIN
	INSERT INTO Core.[Location] (Name, ChurchId, AddressId)
	VALUES
	('189 Church St', 1,2)
END