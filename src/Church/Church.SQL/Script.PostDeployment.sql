/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\_PostDeployment\Core.TimeZone.Populate.sql
:r .\_PostDeployment\Core.Church.Populate.sql
:r .\_PostDeployment\Core.Address.Populate.sql
:r .\_PostDeployment\Core.Location.Populate.sql
