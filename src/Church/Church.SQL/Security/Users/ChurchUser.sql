
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

