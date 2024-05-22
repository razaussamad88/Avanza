/* ---------------------------------------------------------------------------------
:: Created by:  Raza us Samad (razausssamad@gmail.com)
:: Created on:  Tuesday, ‎February ‎18, ‎2014
:: --------------------------------------------------------------------------------- 
*/

Print '--- Refresh VIEWS Query ---'
DECLARE @cmd varchar(1000)
DECLARE view_cursor CURSOR FOR

-- Refresh VIEWS Query
--===============================
SELECT 'EXEC sp_refreshview ''' + TABLE_NAME + ''';' FROM INFORMATION_SCHEMA.Views Order by TABLE_NAME;
--SELECT 'Select ''' + TABLE_NAME + ''';' FROM INFORMATION_SCHEMA.Views Order by TABLE_NAME;

-- Refresh VIEWS Execute
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
/* ========== Refresh VIEWS Query ========== */
Print '--- All View has been Refreshed ---'
Print ' '

Print '--- Re-compile VIEWS Query ---'
DECLARE @cmd varchar(1000)
DECLARE view_cursor CURSOR FOR

-- Re-compile VIEWS Query
--===============================
SELECT 'EXEC sp_recompile ''' + TABLE_NAME + ''';' FROM INFORMATION_SCHEMA.Views Order by TABLE_NAME;

-- Re-compile VIEWS Exe
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
/* ========== Re-compile VIEWS Query ========== */
Print '--- All View has been Re-compiled ---'
Print ' '
