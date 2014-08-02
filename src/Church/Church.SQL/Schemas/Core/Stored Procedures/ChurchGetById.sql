CREATE PROCEDURE [Core].[ChurchGetById]
	@Id INT
AS
	declare @ids dbo.integer_list_tbltype
	insert into @ids ( Id) Values (@Id)

	exec [Core].ChurchGet @ids
