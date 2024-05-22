Note:
	Telerik Server R1 2017 DLL, works with all Templates (.trdp 4.1; .trdp 4.2; .trdx 4.1; .trdx 4.2)
	i.e. Telerik.Reporting.dll 11.0.17.301

Use ODP.Net Managed Driver (For Oracle)


// For SHARED ConnectionString (passing Connection in Telerik Report Constructor)
// it requires  ( Oracle.ManagedDataAccess.Client | System.Data.SqlClient )


// For EMBEDDED ConnectionString (stored in Telerik Report during report template designing)
// it requires to change the ConnectionString whenver report design


	
Readme:
#1: generate trdx with embedded constr (working)
	(version issue. resolved using TextEditor version changed)
	
#2: generate trdp with embedded constr
	(version issue. can be resolve via referencing TR-Server 2017 DLL in .Net Project, instead of using TR-Designer 2016 DLL)


How to generate Telerik Report on 2012 (Reporting R3 2016)

Versions:
		Telerik Report Designer 2016 DLL (10.2.16.1025)
		Telerik Server 2017 DLL (11.0.17.301)
		trdp|trdx	http://schemas.telerik.com/reporting/2012/4.1
		trdp|trdx	http://schemas.telerik.com/reporting/2012/4.2


Open InternetExplorer.
Login to Report Server using url: http://Localhost:83
	LoginId: Administrator
	Pwd:	 Avanza123
	
	-> Go to REPORTS section.
	-> Click NEW REPORTS.
		(report designer will be open on your local machine desktop.)
	-> open your report (trdp, trdx)
	-> now Save the open report on local machine.
		(report is save as )
	
	-> now you have proper versioned template file.
	