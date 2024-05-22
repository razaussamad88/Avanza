Select * FROM  sys.database_files

ALTER DATABASE HBL_NIMBUS SET RECOVERY SIMPLE WITH NO_WAIT
DBCC SHRINKFILE(HBL_NIMBUS_log, 1)
ALTER DATABASE HBL_NIMBUS SET RECOVERY FULL WITH NO_WAIT
GO

Select * FROM  sys.database_files

/* ------------------------------------------------------
	 Shrinking causes fragmentation in the data/indexes. 
	 So you need to re-index after shrinking. 
	 You need free space in the database for the reindex.
*/