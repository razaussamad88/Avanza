@echo off

net stop "Nimbus - Rdv TCPIP Service"
net stop "NIMBUS Config Loader"
net stop "NIMBUS NDC Parser"
net stop "NIMBUS NDC Processor"
net stop "NIMBUS SAF Service"
net stop "NIMBUS TCPIP Service"
pause