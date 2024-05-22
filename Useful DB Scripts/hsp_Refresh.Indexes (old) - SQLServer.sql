

Print '--- Re-organized All table indexes ---'
EXEC sp_MSforeachtable @command1="print '?'", @command2="ALTER INDEX ALL ON ? REORGANIZE"
/* ========== Re-built Indexes ========== */
Print '--- All Indexes has been Re-organized ---'
Print ' '




Print '--- Re-built All table indexes ---'
EXEC sp_MSforeachtable @command1="print '?'", @command2="ALTER INDEX ALL ON ? REBUILD WITH (ONLINE=ON)"
/* ========== Re-built Indexes ========== */
Print '--- All Indexes has been Re-built ---'
Print ' '