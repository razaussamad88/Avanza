@echo off

REM net start "SQL Server (MSSQLSERVER)"

net start "MSSQLSERVER"
net start "MSSQLServerOLAPService"
net start "SSASTELEMETRY"
net start "SQLTELEMETRY"
net start "MsDtsServer150"
net start "SSISTELEMETRY150"
net start "SQLWriter"

pause