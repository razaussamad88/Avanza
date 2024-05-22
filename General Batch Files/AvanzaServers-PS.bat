@echo off

set AvanzaServers='%~dp0AvanzaServers.ps1'


cls
echo.

powershell.exe ". %AvanzaServers% "


rem powershell.exe ". 'C:\Users\Muhammad Farhan\Documents\Script.ps1' "

rem powershell.exe ". 'AvanzaServers.ps1' "

pause
