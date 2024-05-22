

Select * From SYSTEM_CONFIGURATION where PARAM_NAME in 
(
	'REPORT_SERVER_HOST',
	'REPORT_SERVER_USER_NAME',
	'REPORT_SERVER_PASSWORD'
);	
	



-- For TELERIK Username
Update SYSTEM_CONFIGURATION
set PARAM_VALUE='admin'
where PARAM_NAME='REPORT_SERVER_USER_NAME';



-- For TELERIK Hosted Server URL (https)
Update SYSTEM_CONFIGURATION
set PARAM_VALUE='https://localhost:83/'
where PARAM_NAME='REPORT_SERVER_HOST';



-- For reset existing password [new pwd: administrator]
Update SYSTEM_CONFIGURATION
set PARAM_VALUE='v0fbgMkyODqlHHJEHdPTcg=='
where PARAM_NAME='REPORT_SERVER_PASSWORD';



Select * From SYSTEM_CONFIGURATION where REPORT_CON_ID = 'VisionReport';



-- For TELERIK Report Connection String
Update REPORT_CONNECTION_STRING 
set CONNECTION_STRING='Data Source=.;Initial Catalog=SCB_VISION_WEB;User ID=avanza;Password=avanza'
where REPORT_CON_ID='VisionReport';




