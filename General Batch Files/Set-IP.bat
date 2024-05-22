@echo off
REM Created By		:	Muhammad Raza us Samad
REM Creation Date	:	Tuesday, ‎February ‎18, ‎2014

set	ip=128.1.17.112
set	subnet=255.255.255.0
set	gateway=128.1.17.1

echo.
echo Setting IP Address...
echo Param-1 is "%1"

IF (%1) == () (
	echo DHCP
	netsh interface ip set address "Wireless Network Connection" dhcp
	goto End
)

if %1==i (
	echo Static IP: %ip%; Subnet: %subnet%; Gateway: %gateway%
	netsh interface ip set address "Wireless Network Connection" static %ip% %subnet% %gateway%
	goto End
)

:End
echo.
echo.
pause