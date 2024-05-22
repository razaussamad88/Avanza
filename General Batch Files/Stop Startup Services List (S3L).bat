@echo off

REM mode con: cols=105 lines=32766
cls

set "svcFilePath=%~dp0S3L.txt"
set "tmpFile=%temp%\tmp-svc-log.txt"
set "tmpFile_GetKeyName=%temp%\tmp-svc-GetKeyName-log.txt"

REM 		net start > C:\tmp-svc-log.txt

REM net start "Adobe Acrobat Update Service"
REM net stop "Adobe Acrobat Update Service"
REM sc config "Adobe Acrobat Update Service" Start=disabled 
REM sc GetKeyName "Adobe Acrobat Update Service"

echo.

echo F|xcopy "%svcFilePath%" "%tmpFile%" /S /Y /F

echo.
echo.


echo *** Stopping Services to be Disabled...
echo ****************************************************************************************************
echo.
FOR /F "tokens=*" %%i IN (%tmpFile%) DO (
	Echo %%i
	net STOP "%%i"
	)

echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Generating Service's KeyNames...
echo ****************************************************************************************************
echo.
(
FOR /F "tokens=*" %%i IN (%tmpFile%) DO (
	Echo %%i
	sc GetKeyName "%%i"
	)
) > %tmpFile_GetKeyName%

echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Configuring Services...
echo ****************************************************************************************************
echo.
FOR /F "tokens=2,3 delims= " %%a IN (%tmpFile_GetKeyName%) DO (
	
	if %%a=== (
		echo ++++++++ Service [%%b] updating config to  [StartupAs : DISABLED]
		sc config %%b Start=disabled
		) 
	)

echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo.

pause
cls