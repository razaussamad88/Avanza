
IF OBJECT_ID('helper_VW_RENAME_COLUMNS', 'V') IS NOT NULL
	Drop View helper_VW_RENAME_COLUMNS
GO

/* Execute on VSN */
Create View helper_VW_RENAME_COLUMNS AS
	select t.Name, 'exec sp_RENAME N''' + t.name + '.' + QUOTENAME(c.Column_Name) + ''', ''' + UPPER(c.Column_Name) + ''', ''COLUMN'';' As Command
	from sys.all_objects t 
	INNER JOIN information_schema.COLUMNS c ON c.TABLE_NAME = t.Name
	where t.type in ('u','v')
	-- AND t.name = 'VW_MPG_CREDIT_BATCH_SEARCH'
	-- ORDER BY t.name
GO