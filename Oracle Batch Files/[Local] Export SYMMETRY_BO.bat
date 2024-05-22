@echo off
cls

REM NOTE: MUST HAVE TNS IN ON MACHINE YOU RUNNING. EXPORT WILL GENERATE ON LOCAL MACHINE (NOT ON SERVER).


REM *****  SET FILE NAME  *************************
set "expBasePath=D:\OrcaleExportedDatabases\"
set "filename=SYMMETRY_BO_"
REM ***********************************************



for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

set "datestamp=%YYYY%%MM%%DD%" & set "timestamp=%HH%%Min%%Sec%"
set "fullstamp=%datestamp%_%timestamp%"
set "expFilePath=%expBasePath%%filename%%fullstamp%.dmp"
set "expLogFilePath=%expBasePath%%filename%%fullstamp%.log"

REM SELECT 'ALTER TABLE '||table_name||' ALLOCATE EXTENT;' FROM user_tables WHERE segment_created = 'NO';
REM SELECT * FROM V$VERSION;
REM SELECT version FROM V$INSTANCE;  
REM SELECT directory_path FROM dba_directories WHERE directory_name = 'DATA_PUMP_DIR';


REM exp vision_web/Vision123$@ICM_WEB file='E:\Databases\BBK_vision_SIT.dmp' owner=vision_web


REM exp BBK_VISION_WEB/Vision123$@ORCL_LOCAL_xx127 file='D:\OracleDatabases\Export\BBK_VISION_WEB_TEST_SCHEMA.dmp' owner=BBK_VISION_WEB rows=n
REM ... the ROWS=N parameter tells Oracle not to export any data. simple imp can be use to import above rows=n command.

exp SYMMETRY_BO/symmetry@ORCL_LOCAL_xx127 file='%expFilePath%'  log='%expLogFilePath%' owner=SYMMETRY_BO COMPRESS=y

REM expdp exports on server machine
REM expdp Vision_Web/Vision123$@10.2.0.82:1521/rdvsit directory=DATA_PUMP_DIR dumpfile=DUMPFILE.dmp logfile=DUMP.log owner=Vision_Web VERSION=11.1

REM exp VISION_MIGRATION2/VISION_MIGRATION2@localhost:1521/orcl file='%expFilePath%'  log='%expLogFilePath%' owner=VISION_MIGRATION2 
REM exp Vision_Web2/Vision_Web2@localhost:1521/orcl file='%expFilePath%'  log='%expLogFilePath%' owner=Vision_Web2 constraints=n indexes=n statistics=none
REM expdp Vision_Web2/Vision_Web2@localhost:1521/orcl directory=DATA_PUMP_DIR dumpfile=DUMPFILE.dmp logfile=DUMP.log owner=Vision_Web2 VERSION=11.1

REM expdp Vision_Web2/Vision_Web2@localhost:1521/orcl directory=DATA_PUMP_DIR dumpfile=BBK_SIT_LOCAL_VISION_3Aug2017.dmp logfile=BBK_SIT_LOCAL_VISION_3Aug2017.log schemas=Vision_Web2 content=DATA_ONLY

rem		new oracle user-id: 			vision_web
rem		new user's password: 			Vision123$
rem		TNS Name:						ICM_WEB
rem		new export file location:		E:\Databases\BBK_vision_SIT.dmp
rem		database to be export from dmp:	owner=vision_web
rem		able to import in 11.1 oracle:	VERSION=11.1 only

pause