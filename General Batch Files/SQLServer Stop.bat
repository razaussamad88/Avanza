@echo off

REM net stop "SQL Server (MSSQLSERVER)"

net stop "MSSQLSERVER"
net stop "MSSQLServerOLAPService"
net stop "SSASTELEMETRY"
net stop "SQLTELEMETRY"
net stop "MsDtsServer150"
net stop "SSISTELEMETRY150"
net stop "SQLWriter"


pause