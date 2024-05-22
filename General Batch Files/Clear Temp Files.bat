@echo off

cls
echo.
echo.
echo.

echo ## Removing Microsoft.NET Framework x64 Temp Files...
echo ############################################################
del /s /f /q "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files"
echo Done!!
echo.

echo ## Removing Microsoft.NET Framework x86 Temp Files...
echo ############################################################
del /s /f /q "C:\Windows\Microsoft.NET\Framework\v4.0.30319\Temporary ASP.NET Files"
echo Done!!
echo.

echo ## Removing Windows Temp Files...
echo ############################################################
del /s /f /q "C:\Windows\Temp"
echo Done!!
echo.

echo ## Removing User Temp Files...
echo ############################################################
del /s /f /q %temp%
echo Done!!
echo.

echo.
echo.
echo.

pause
cls