@echo off

cls
echo.
echo.
echo.

echo *** Connecting Avanza Repo Servers...
echo ****************************************************************************************************
echo :::  SVN  :::
ping svn1.avanzasolutions.com
echo.
echo.
echo :::  GitHub  :::
ping git-hub.avanza.pk
echo.
echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Connecting Avanza Jenkins Server...
echo ****************************************************************************************************
ping jenkins1.avanzasolutions.com
echo.
echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Connecting Avanza Phabricator Server...
echo ****************************************************************************************************
ping phabricator1.avanzasolutions.com
echo.
echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Connecting Avanza Oracle Servers...
echo ****************************************************************************************************
echo :::  Rdv Notification  :::
ping 172.16.5.71
echo.
echo.
echo :::  ATG PD Team  :::
ping 172.16.0.75
echo.
echo.
echo :::  ATG Mobile Team  :::
ping 172.16.0.48
echo.
echo.
echo -----  Completed!!  -----
echo ####################################################################################################
echo.
echo.

echo.
echo.
echo *** Connecting Avanza SQL Servers...
echo ****************************************************************************************************
echo :::  ATG-SQL2012  :::
ping 172.16.0.74
echo.
echo.
echo :::  ATG-SQL2014  :::
ping 172.16.0.64
echo.
echo.
echo :::  ATG-SQL2017  :::
ping 172.16.0.94
echo.
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
