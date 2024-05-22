



-- OldSchemaName : RDV_DESIGNER
-- NewSchemaName : dbo
SELECT DbObjects.Type, 'ALTER SCHEMA dbo TRANSFER [' + SysSchemas.Name + '].[' + DbObjects.Name + '];'
FROM sys.Objects DbObjects
INNER JOIN sys.Schemas SysSchemas ON DbObjects.schema_id = SysSchemas.schema_id
WHERE SysSchemas.Name = 'RDV_DESIGNER'
-- AND (DbObjects.Type IN ('U', 'P', 'V'))