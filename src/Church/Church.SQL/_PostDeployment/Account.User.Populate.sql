IF NOT EXISTS (SELECT 1 FROM Account.[User] a WHERE a.Id = 1)
BEGIN
	INSERT INTO Account.[User] (PersonId, HashedPassword, Salt, IsActive)
	VALUES
	(
		1,
		'xxx',
		'yyy',
		1
	)
END