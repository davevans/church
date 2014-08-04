
CREATE USER [ChurchUser] 
  	   FOR LOGIN [ChurchUser] 
	   WITH DEFAULT_SCHEMA=[dbo]
GO

GRANT CONNECT TO [ChurchUser]
GO
GRANT SELECT ON SCHEMA :: Core To [ChurchUser]
GO
GRANT INSERT ON SCHEMA :: Core To [ChurchUser]
GO
GRANT UPDATE ON SCHEMA :: Core To [ChurchUser]
GO
GRANT DELETE ON SCHEMA :: Core To [ChurchUser]
GO



/*
GRANTS FOR STORED PROCEDURES
*/

GRANT EXECUTE ON [Core].ChurchGet TO [ChurchUser] As [dbo];
GO
GRANT EXECUTE ON [Core].ChurchGetById TO [ChurchUser] As [dbo];
GO
GRANT EXECUTE ON [Core].ChurchInsert TO [ChurchUser] As [dbo];
GO
GRANT EXECUTE ON [Core].ChurchUpdate TO [ChurchUser] As [dbo];


GO
GRANT EXECUTE ON [Core].PersonGet TO [ChurchUser] As [dbo];
GO
GRANT EXECUTE ON [Core].[PersonGetByChurchId] TO [ChurchUser] As [dbo];
GO

