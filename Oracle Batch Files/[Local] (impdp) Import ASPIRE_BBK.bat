@echo off
cls

REM NOTE: MUST HAVE TNS IN ON MACHINE YOU RUNNING. EXPORT WILL GENERATE ON LOCAL MACHINE (NOT ON SERVER).


REM *****  SET FILE NAME  *************************
REM set "impBasePath=D:\OracleDatabases\ASPIRE_BBK\"
set "filename=ASPIRE_BBK"
REM ***********************************************



for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

set "datestamp=%YYYY%%MM%%DD%" & set "timestamp=%HH%%Min%%Sec%"
set "fullstamp=%datestamp%_%timestamp%"
REM set "impFilePath=%impBasePath%%filename%.dmp"
REM set "impLogFilePath=%impBasePath%%filename%%fullstamp%.log"

REM SELECT 'ALTER TABLE '||table_name||' ALLOCATE EXTENT;' FROM user_tables WHERE segment_created = 'NO';
REM SELECT * FROM V$VERSION;
REM SELECT version FROM V$INSTANCE;  
REM SELECT directory_path FROM dba_directories WHERE directory_name = 'DATA_PUMP_DIR';
REM directory_path = D:\app\oracle12c\admin\orcl\dpdump
REM directory = DATA_PUMP_DIR = directory_path


REM imp ASPIRE_BBK/aspire@10.2.0.82:1521/rdvsit file='%impFilePath%'  log='%impLogFilePath%' FROMUSER=ASPIRE_BBK
REM imp ASPIRE_BBK/aspire@ORCL_LOCAL_xx127 file='%impFilePath%'  log='%impLogFilePath%' FROMUSER=ASPIRE_BBK

impdp aspire_bbk/aspire@ORCL_LOCAL_xx127 directory=DATA_PUMP_DIR dumpfile=%filename%.dmp logfile=%filename%_%fullstamp%.log 


rem		new oracle user-id: 			aspire_bbk
rem		new user's password: 			aspire
rem		TNS Name:						ORCL_LOCAL_xx127
rem		new import file location:		D:\app\oracle12c\admin\orcl\dpdump\ASPIRE_BBK.dmp

pause