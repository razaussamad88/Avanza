/* ---------------------------------------------------------------------------------
:: Created by:  Raza us Samad (razausssamad@gmail.com)
:: Created on:  Tuesday, ‎February ‎18, ‎2014
:: --------------------------------------------------------------------------------- 
*/

Print '--- Re-organize INDEXES Query ---'
DECLARE @cmd varchar(1000)
DECLARE view_cursor CURSOR FOR

-- Re-organized INDEXES Query
--===============================
SELECT distinct * 
FROM
(
	SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REORGANIZE;' as [COL] FROM INFORMATION_SCHEMA.Views UNION ALL
	SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REORGANIZE;' as [COL] FROM INFORMATION_SCHEMA.TABLES
)[TBL]
Order by 1;

-- SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REORGANIZE;' FROM INFORMATION_SCHEMA.Views Order by TABLE_NAME;
-- EXEC sp_MSforeachtable @command1="print '?'", @command2="ALTER INDEX ALL ON ? REORGANIZE"

-- Re-organized INDEXES Execute
--===============================
OPEN view_cursor

-- Perform the first fetch.
FETCH NEXT FROM view_cursor INTO @cmd

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN TRY
		-- Execute Command
		print @cmd
		exec (@cmd)
	
	END TRY
	BEGIN CATCH
		print '*** ERROR OCCURED *** : ' + @cmd
	END CATCH

	-- This is executed as long as the previous fetch succeeds.
	FETCH NEXT FROM view_cursor INTO @cmd
END

CLOSE view_cursor
DEALLOCATE view_cursor
GO
/* ========== Re-organized INDEXES Query ========== */
Print '--- All Indexes has been Re-organized ---'
Print ' '


Print '--- Re-build INDEXES Query ---'
DECLARE @cmd varchar(1000)
DECLARE view_cursor CURSOR FOR

-- Re-built VIEWS Query
--===============================
SELECT distinct * 
FROM
(
	SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REBUILD WITH (ONLINE=ON);' as [COL] FROM INFORMATION_SCHEMA.Views UNION ALL
	SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REBUILD WITH (ONLINE=ON);' as [COL] FROM INFORMATION_SCHEMA.TABLES
)[TBL]
Order by 1;
-- SELECT 'ALTER INDEX ALL ON ' + TABLE_NAME + ' REBUILD WITH (ONLINE=ON);' FROM INFORMATION_SCHEMA.Views Order by TABLE_NAME;
-- EXEC sp_MSforeachtable @command1="print '?'", @command2="ALTER INDEX ALL ON ? REBUILD WITH (ONLINE=ON)"

-- Re-built VIEWS Execute
--===============================
OPEN view_cursor

-- Perform the first fetch.
FETCH NEXT FROM view_cursor INTO @cmd

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN TRY
		-- Execute Command
		--print @cmd
		exec (@cmd)
	
	END TRY
	BEGIN CATCH
		print '*** ERROR OCCURED *** : ' + @cmd
	END CATCH

	-- This is executed as long as the previous fetch succeeds.
	FETCH NEXT FROM view_cursor INTO @cmd
END

CLOSE view_cursor
DEALLOCATE view_cursor
GO
/* ========== Re-built INDEXES Query ========== */
Print '--- All Indexes has been Re-built ---'
Print ' '
