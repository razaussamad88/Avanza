
DROP VIEW IF EXISTS helper_VW_GET_TABLES
GO
-- :::.. USER_TABLE ..:::
CREATE VIEW helper_VW_GET_TABLES AS
	SELECT 
		TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH,
		NUMERIC_PRECISION, NUMERIC_SCALE, CHARACTER_SET_NAME, COLLATION_NAME
	FROM	INFORMATION_SCHEMA.COLUMNS C with(nolock)
	Inner Join sys.all_objects O with(nolock) on (C.TABLE_NAME = O.name and O.type = 'U')
	WHERE	TABLE_CATALOG = DB_NAME();
	-- Order by TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION;
GO




DROP VIEW IF EXISTS helper_VW_GET_VIEWS
GO
-- :::.. VIEW ..:::
CREATE VIEW helper_VW_GET_VIEWS AS
	SELECT 
		TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH,
		NUMERIC_PRECISION, NUMERIC_SCALE, CHARACTER_SET_NAME, COLLATION_NAME
	FROM	INFORMATION_SCHEMA.COLUMNS C with(nolock)
	Inner Join sys.all_objects O with(nolock) on (C.TABLE_NAME = O.name and O.type = 'V')
	WHERE	TABLE_CATALOG = DB_NAME();
	-- Order by TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION;
GO




DROP VIEW IF EXISTS helper_VW_GET_CONSTRAINTS
GO
-- :::.. USER_TABLE CONSTRAINTS ..:::
CREATE VIEW helper_VW_GET_CONSTRAINTS AS
	SELECT	--DB_NAME() [CONSTRAINT_CATALOG],
			SCHEMA_NAME(O.schema_id) [CONSTRAINT_SCHEMA],

			OBJECT_NAME(O.parent_object_id) [TABLE_NAME], 
			O.name [CONSTRAINT_NAME],
			O.type [TYPE], 
			O.type_desc [TYPE_DESC]

	FROM	sys.all_objects O with(nolock)
	WHERE	O.type in ('D','F','PK','UQ')
	-- Order by 3, 5 desc, 4;
GO


/* 
Select distinct type, type_desc From sys.all_objects order by type_desc;
RESULT ::->>
	D 	DEFAULT_CONSTRAINT
	F 	FOREIGN_KEY_CONSTRAINT
	FN	SQL_SCALAR_FUNCTION
	P 	SQL_STORED_PROCEDURE
	PK	PRIMARY_KEY_CONSTRAINT
	TF	SQL_TABLE_VALUED_FUNCTION
	TR	SQL_TRIGGER
	U 	USER_TABLE
	UQ	UNIQUE_CONSTRAINT
	V 	VIEW
*/




DROP VIEW IF EXISTS helper_VW_GET_DB_VERSION
GO
-- :::.. USER_TABLE ..:::
CREATE VIEW helper_VW_GET_DB_VERSION AS
	SELECT
	  CASE 
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '8%' THEN 'SQL2000'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '9%' THEN 'SQL2005'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '10.0%' THEN 'SQL2008'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '10.5%' THEN 'SQL2008 R2'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '11%' THEN 'SQL2012'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '12%' THEN 'SQL2014'
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '13%' THEN 'SQL2016'     
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '14%' THEN 'SQL2017' 
		 WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) like '15%' THEN 'SQL2019' 
		 ELSE 'unknown'
	  END AS MajorVersion,
	  SERVERPROPERTY('ProductLevel') AS ProductLevel,
	  SERVERPROPERTY('Edition') AS Edition,
	  SERVERPROPERTY('ProductVersion') AS ProductVersion,
	  DB_NAME() AS CatalogName;
GO
/*	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~     O U T P U T     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~	*/
/*
	MajorVersion	ProductLevel	Edition	ProductVersion
	SQL2016	RTM	Enterprise Edition (64-bit)	13.0.1601.5
	SQL2017	RTM	Developer Edition (64-bit)	14.0.1000.169
*/




DROP VIEW IF EXISTS helper_VW_GET_COLUMN_INDEXES
GO
-- :::.. USER_TABLE KEY COLUMN INDEXES ..:::
CREATE VIEW helper_VW_GET_COLUMN_INDEXES AS
	SELECT 
		 OBJECT_SCHEMA_NAME(T.[object_id], DB_ID()) AS [Schema], 
		 TableName = t.name,
		 IndexName = ind.name,
		 ColumnName = c.name,
		 IndexType = ind.type_desc,
		 IsPrimaryKey = ind.is_primary_key,
		 IsUnique = ind.is_unique,
		 IsDescendingKey = IC.[is_descending_key],
		 KeyOrdinal = ic.key_ordinal
	FROM sys.tables t with(nolock)
	inner JOIN sys.all_columns c with(nolock) ON c.object_id = t.object_id 
	inner JOIN sys.indexes ind with(nolock) ON ind.object_id = t.object_id
	inner JOIN sys.index_columns ic with(nolock) ON  (t.object_id = ic.object_id and ind.index_id = ic.index_id and c.column_id = ic.column_id)
	-- ORDER BY t.name, ind.name, ic.key_ordinal;
GO




DROP VIEW IF EXISTS helper_VW_GET_IDENTITY_COLUMNS
GO
-- :::.. USER_TABLE IDENTITY COLUMNS ..:::
CREATE VIEW helper_VW_GET_IDENTITY_COLUMNS AS
	SELECT 
		[Schema] = OBJECT_SCHEMA_NAME([object_id], DB_ID()),  
		[TableName] = OBJECT_NAME([object_id]),
		[ColumnName] = name,
		[SeedValue] = seed_value, 
		[IncrementValue] = increment_value,
		[Remarks] = 
			Case
				When seed_value is null OR increment_value is null Then 'Non-Key Identity Column'
				Else 'Key Identity Column'
			End 
	FROM sys.identity_columns
	-- WHERE OBJECT_SCHEMA_NAME([object_id], DB_ID()) <> 'sys'
	-- Order By 1, 2, 3;
GO


/*
	Select * From helper_VW_GET_DB_VERSION with(nolock);

	Select * From helper_VW_GET_TABLES with(nolock) Order by TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION;
	Select * From helper_VW_GET_VIEWS with(nolock) Order by TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION;

	Select * From helper_VW_GET_CONSTRAINTS with(nolock) Order by TABLE_NAME, [TYPE] desc, CONSTRAINT_NAME;
	Select * From helper_VW_GET_COLUMN_INDEXES with(nolock) ORDER BY TableName, IsPrimaryKey Desc, IsUnique Desc, IndexName, KeyOrdinal;

	Select * From helper_VW_GET_IDENTITY_COLUMNS with(nolock) ORDER BY [Schema], TableName, ColumnName;
*/

	
