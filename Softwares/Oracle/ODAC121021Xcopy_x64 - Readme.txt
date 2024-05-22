

ORACLE_HOME_NAME (look in this file)
	-> C:\Program Files\Oracle\Inventory\ContentsXML\inventory.xml
		 OraDB12Home1

echo %ORACLE_HOME%
	-> "D:\app\muhammad.raza\product\12.1.0\dbhome_1"

====================================================================================================

::: Run Below Commands
----------------------

cd ODAC121021Xcopy_x64


X:\ODAC121021Xcopy_x64>
	install.bat all "D:\app\muhammad.raza\product\12.1.0\dbhome_1" OraDB12Home1 true
	install.bat all "D:\app\muhammad.raza\product\12.1.0\dbhome_1" ODAC true
	
	Uninstall.bat all ODAC	
	uninstall.bat all "D:\app\muhammad.raza\product\12.1.0\dbhome_1" OraDB12Home1 true
	
	
X:\ODAC121021Xcopy_x64>
	configure.bat all OraDB12Home1 true
	
	or
	
	configure.bat odp.net2 OraDB12Home1 true
	configure.bat odp.net4 OraDB12Home1 true
	configure.bat basic OraDB12Home1 true
	configure.bat oledb OraDB12Home1 true
	
	
	
	
	unconfigure.bat all OraDB12Home1 true


====================================================================================================	
	lsnrctl status
	lsnrctl stop
	lsnrctl start
	
	
	sqlplus sys/sys@ORCL_LOCAL_xx127 as sysdba;
	
	
Oracle Universal Installer
	D:\app\muhammad.raza\product\12.1.0\dbhome_1\oui\bin\setup.exe
	
	