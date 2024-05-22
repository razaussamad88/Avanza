

/* ================================= */
/* ================================= */
--		SEC_GROUP_PERMISSION
/* ================================= */

Delete FROM SEC_GROUP_PERMISSION WHERE PERMISSION_ID not in (
	Select PERMISSION_ID FROM SEC_PERMISSION
);


-- SELECT * FROM SEC_GROUP_PERMISSION where group_ID = 'admin'
Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'atmoperations%'
	or	PERMISSION_ID like 'authentication%'
	or	PERMISSION_ID like 'cardproduction%'
	or	PERMISSION_ID like 'complaintmanagement%'
	or	PERMISSION_ID like 'electronics%'
	or	PERMISSION_ID like 'hk%'
	or	PERMISSION_ID like 'mastercardsettlement%'
	or	PERMISSION_ID like 'notification%'
	or	PERMISSION_ID like 'offerandpromotion%'
	or	PERMISSION_ID like 'operations%'
	or	PERMISSION_ID like 'posmanagement%'
	or	PERMISSION_ID like 'prepaidcard%'
	or	PERMISSION_ID like 'regulation%'
	or	PERMISSION_ID like 'utilitybillmanagement%'
	or	PERMISSION_ID like 'visacardsettlement%'
	or	PER_PARENT_ID like 'cardproduction%'
	or	PER_PARENT_ID like 'complaintmanagement%'
	or	PER_PARENT_ID like 'electronics%'
	or	PER_PARENT_ID like 'hk%'
	or	PER_PARENT_ID like 'mastercardsettlement%'
	or	PER_PARENT_ID like 'notification%'
	or	PER_PARENT_ID like 'offerandpromotion%'
	or	PER_PARENT_ID like 'operations%'
	or	PER_PARENT_ID like 'posmanagement%'
	or	PER_PARENT_ID like 'prepaidcard%'
	or	PER_PARENT_ID like 'regulation%'
	or	PER_PARENT_ID like 'utilitybillmanagement%'
	or	PER_PARENT_ID like 'visacardsettlement%'
);



Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'security.dashboarddesigner%'
	or	PERMISSION_ID like 'security.managekeyspasswords%'
	or	PERMISSION_ID like 'security.userdashboard%'
);



Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'switchoperations.commandprocessor%'
	or	PERMISSION_ID like 'switchoperations.scheduler%'
	or	PERMISSION_ID like 'switchoperations.stip%'
);



Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like '%monitoring.atmdashboard%'
	or	PERMISSION_ID like 'monitoring.transactiondashboard%'
	or	PERMISSION_ID like 'monitoring.wallboard%'
	or	PERMISSION_ID like 'monitoring.customerdashboard%'
);



Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION
	where (
		PERMISSION_ID like 'checker-%'
	or	PERMISSION_ID like 'checker.%')
	and	PERMISSION_ID not like 'checker.users%'
	and PERMISSION_ID not like 'checker.groups%'
);



Delete
-- SELECT *
from SEC_GROUP_PERMISSION where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION
	where (
		PERMISSION_ID like '%CustomDashboard%'
	or	PERMISSION_ID like 'security.userpreference%')
);




/* ================================= */
/* ================================= */
--		SEC_APP_PERMISSION_MAP
/* ================================= */

-- SELECT * FROM SEC_GROUP_PERMISSION where group_ID = 'admin'
Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'atmoperations%'
	or	PERMISSION_ID like 'authentication%'
	or	PERMISSION_ID like 'cardproduction%'
	or	PERMISSION_ID like 'complaintmanagement%'
	or	PERMISSION_ID like 'electronics%'
	or	PERMISSION_ID like 'hk%'
	or	PERMISSION_ID like 'mastercardsettlement%'
	or	PERMISSION_ID like 'notification%'
	or	PERMISSION_ID like 'offerandpromotion%'
	or	PERMISSION_ID like 'operations%'
	or	PERMISSION_ID like 'posmanagement%'
	or	PERMISSION_ID like 'prepaidcard%'
	or	PERMISSION_ID like 'regulation%'
	or	PERMISSION_ID like 'utilitybillmanagement%'
	or	PERMISSION_ID like 'visacardsettlement%'
	or	PER_PARENT_ID like 'cardproduction%'
	or	PER_PARENT_ID like 'complaintmanagement%'
	or	PER_PARENT_ID like 'electronics%'
	or	PER_PARENT_ID like 'hk%'
	or	PER_PARENT_ID like 'mastercardsettlement%'
	or	PER_PARENT_ID like 'notification%'
	or	PER_PARENT_ID like 'offerandpromotion%'
	or	PER_PARENT_ID like 'operations%'
	or	PER_PARENT_ID like 'posmanagement%'
	or	PER_PARENT_ID like 'prepaidcard%'
	or	PER_PARENT_ID like 'regulation%'
	or	PER_PARENT_ID like 'utilitybillmanagement%'
	or	PER_PARENT_ID like 'visacardsettlement%'
);



Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'security.dashboarddesigner%'
	or	PERMISSION_ID like 'security.managekeyspasswords%'
	or	PERMISSION_ID like 'security.userdashboard%'
);



Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like 'switchoperations.commandprocessor%'
	or	PERMISSION_ID like 'switchoperations.scheduler%'
	or	PERMISSION_ID like 'switchoperations.stip%'
);



Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION where 
		PERMISSION_ID like '%monitoring.atmdashboard%'
	or	PERMISSION_ID like 'monitoring.transactiondashboard%'
	or	PERMISSION_ID like 'monitoring.wallboard%'
	or	PERMISSION_ID like 'monitoring.customerdashboard%'
);



Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION
	where (
		PERMISSION_ID like 'checker-%'
	or	PERMISSION_ID like 'checker.%')
	and	PERMISSION_ID not like 'checker.users%'
	and PERMISSION_ID not like 'checker.groups%'
);



Delete
-- SELECT *
from SEC_APP_PERMISSION_MAP where PERMISSION_ID in (
	SELECT PERMISSION_ID
	from SEC_PERMISSION
	where (
		PERMISSION_ID like '%CustomDashboard%'
	or	PERMISSION_ID like 'security.userpreference%')
);




/* ================================= */
/* ================================= */
--		SEC_GROUP
/* ================================= */


Update SEC_GROUP
Set HOME_PAGE = 'security.groups'
WHERE HOME_PAGE = 'security.userdashboard'
OR HOME_PAGE is null;


DELETE 
-- SELECT *
FROM SEC_USER_BRANCH WHERE 
LOGIN_ID not like 'superuser%'
AND LOGIN_ID not like 'mpguser%'
AND LOGIN_ID not like 'admin';


DELETE 
-- SELECT *
FROM SEC_USER_ENTITY WHERE 
LOGIN_ID not like 'superuser%'
AND LOGIN_ID not like 'mpguser%'
AND LOGIN_ID not like 'admin';


DELETE 
-- SELECT *
FROM SEC_USER_GROUP WHERE 
LOGIN_ID not like 'superuser%'
AND LOGIN_ID not like 'mpguser%'
AND LOGIN_ID not like 'admin';


TRUNCATE TABLE SEC_OAUTH_TOKEN;
TRUNCATE TABLE DASHBORAD_WIDGETS_MAPPING;
DELETE FROM DASHBORAD_USER_PAGES;
TRUNCATE TABLE SEC_PWD_HISTORY;


DELETE 
-- SELECT *
FROM SEC_USER WHERE 
LOGIN_ID not like 'superuser%'
AND LOGIN_ID not like 'mpguser%'
AND LOGIN_ID not like 'admin';


Delete 
-- SELECT *
FROM SEC_GROUP_PERMISSION WHERE GROUP_ID not in (
	Select GROUP_ID FROM SEC_USER_GROUP
);


Delete 
-- SELECT *
FROM SEC_GROUP WHERE GROUP_ID not in (
	Select GROUP_ID FROM SEC_USER_GROUP
);



Update SEC_USER_GROUP
Set GROUP_ID = 'superadmin'
WHERE LOGIN_ID = 'superuser2';


Delete 
-- SELECT *
FROM SEC_GROUP WHERE GROUP_ID not in (
	Select GROUP_ID FROM SEC_USER_GROUP
);




/* ================================= */
/* ================================= */
--		SEC_GROUP
/* ================================= */


Delete 
-- SELECT *
FROM SEC_POLICY
WHERE 
	POLICY_ID not in (Select POLICY_ID FROM SEC_USER)
AND POLICY_ID not in ('admin-policy', 'user-policy');


Delete 
-- SELECT *
FROM BRANCH 
WHERE BRANCH_CODE not in (Select BRANCH_CODE FROM SEC_USER_BRANCH)
AND BRANCH_CODE not in ('0000', '1');


Truncate table SEC_MANAGE_PASSWORDS_KEYS;
Truncate table SEC_USER_SESSION_LOG;


DELETE FROM ALERT_GROUP_USERS;
DELETE FROM ALERT_GROUP;


Truncate table AUDIT_LOG_FIELD;
DELETE FROM AUDIT_LOG_ENTITY;
DELETE FROM AUDIT_LOG_BATCH;


DELETE FROM Blogs;
Truncate table STIP_LOG;
Truncate table IMPORT_LOG_ERROR;
DELETE FROM IMPORT_LOG;
Truncate table BATCH_JOB_LOG;


TRUNCATE TABLE CARD_EXPORT_EXE;
TRUNCATE TABLE CARD_IMPORT_EXE;
TRUNCATE TABLE CARD_LIMIT_ACTUAL;
TRUNCATE TABLE CARD_LIMIT_EXCEP;
TRUNCATE TABLE CARD_PERMISSION_PROFILE;
TRUNCATE TABLE CARD_LIMIT_PROFILE;
TRUNCATE TABLE CARD_REQUEST;
TRUNCATE TABLE CARD_REQUEST_BULK;
TRUNCATE TABLE DEBIT_CARD_PRODUCT_IMD;

TRUNCATE TABLE PROD_CARD_TYPE_MAIN_FORMAT;
TRUNCATE TABLE DEBIT_CARD_TYPE_SEQUENCE;
TRUNCATE TABLE CARD_TYPE_PAN_FORMAT;
TRUNCATE TABLE ENTITY_CARD_TYPE_MAPPING;
TRUNCATE TABLE REGULATION_DEBIT_CARD;

DELETE FROM DEBIT_CARD_TYPE;
DELETE FROM DEBIT_CARD_PRODUCT;


TRUNCATE TABLE ACCT_NOTIFICATION_SUBSCRIBTION;


TRUNCATE TABLE CUSTOMER_RELATIONSHIP_INFO;
TRUNCATE TABLE UTILITY_CONSUMER_CUSTOMER;
TRUNCATE TABLE CUSTOMER_APP_SUBSCRIBTION;
TRUNCATE TABLE CUSTOMER_LIMIT_EXCEP;
TRUNCATE TABLE CUSTOMER_LIMIT_PROFILE;


TRUNCATE TABLE DATA_CHECKER_DETAIL;
DELETE FROM DATA_CHECKER_MAIN;
TRUNCATE TABLE DEBIT_CARD_SEQUENCE_MAPPING;




/*
-- Select 'Select Count(1) as [' + TABLE_NAME + '] From ' + TABLE_NAME From INFORMATION_SCHEMA.TABLES
Select 'Select * From ' + TABLE_NAME From INFORMATION_SCHEMA.TABLES
Where TABLE_NAME like '%log%'
AND TABLE_TYPE = 'BASE TABLE'
*/