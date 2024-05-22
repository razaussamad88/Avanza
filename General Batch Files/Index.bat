@echo off
cls
rem ---------------------------------------------------------------------------------
rem Created by:  Raza us Samad (razausssamad@yahoo.com)
rem Created on:  17th May 2013
rem Description: Batch file to execute shortcuts.
rem ---------------------------------------------------------------------------------

rem ---------------------------------------------------------------------------------
rem Switcher
rem ---------------------------------------------------------------------------------
if "%1"=="" (
	Call cmd
)
if "%1"=="-s" (
	echo Open Vision Server Path
	%SystemRoot%\explorer.exe "C:\Inetpub\wwwroot\Vision"
	goto End
)
if "%1"=="-c" (
	echo Open Vision Client Path
	%SystemRoot%\explorer.exe "%UserProfile%\Local Settings\Apps\2.0"
	goto End
)
if "%1"=="-t" (
	echo Open Temporary ASP.NET Files Path
	%SystemRoot%\explorer.exe "%windir%\Microsoft.NET\Framework\v2.0.50727\Temporary ASP.NET Files"
	goto End
)
if not "%1"=="-s" if not "%1"=="-c" if not "%1"=="-t" (
	goto HelpSec
)


rem ---------------------------------------------------------------------------------
rem Sections
rem ---------------------------------------------------------------------------------
:HelpSec
echo Help:
echo --------------------------------------------------------------
echo Vision.bat [S^|C]:
echo 	-S : goto Vision Server folder
echo 	-C : goto Vision Client folder
goto End

:End