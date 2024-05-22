@echo off

net start "Nimbus - Rdv TCPIP Service"
net start "NIMBUS Config Loader"
net start "NIMBUS NDC Parser"
net start "NIMBUS NDC Processor"
net start "NIMBUS SAF Service"
net start "NIMBUS TCPIP Service"
pause