@echo off
cls

cd C:\app\muhammad.raza\product\12.1.0\dbhome_1\BIN\

echo 'Press ANY key to drop User [MPG_VISIONWEB] and re-Create it...'

pause


REM =============================================================
REM 			MPG_VISIONWEB
REM =============================================================
sqlplus sys/sys as sysdba;

drop user MPG_VISIONWEB cascade;

create user MPG_VISIONWEB identified by vision;
grant connect, unlimited tablespace, resource, dba to MPG_VISIONWEB;
grant create session to MPG_VISIONWEB;
grant sysdba to MPG_VISIONWEB;

quit;


REM =============================================================
REM 			RDV_UAT
REM =============================================================
sqlplus sys/sys as sysdba;

drop user RDV_UAT cascade;

create user RDV_UAT identified by rdv;
grant connect, unlimited tablespace, resource, dba to RDV_UAT;
grant create session to RDV_UAT;
grant sysdba to RDV_UAT;

quit;


REM =============================================================
REM 			NIMBUS_BASE
REM =============================================================
sqlplus sys/sys as sysdba;

drop user NIMBUS_BASE cascade;

create user NIMBUS_BASE identified by nimbus;
grant connect, unlimited tablespace, resource, dba to NIMBUS_BASE;
grant create session to NIMBUS_BASE;
grant sysdba to NIMBUS_BASE;

quit;


REM =============================================================
REM 			RDV_NOTIFICATION
REM =============================================================
sqlplus sys/sys as sysdba;

drop user RDV_NOTIFICATION cascade;

create user RDV_NOTIFICATION identified by rdv;
grant connect, unlimited tablespace, resource, dba to RDV_NOTIFICATION;
grant create session to RDV_NOTIFICATION;
grant sysdba to RDV_NOTIFICATION;

quit;

