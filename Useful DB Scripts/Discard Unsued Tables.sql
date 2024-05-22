

DROP TABLE __MigrationHistory;


Select * From sys.all_objects where name like '%29SEP2021%' and type ='U'
DROP TABLE _BRANCH_29SEP2021;
DROP TABLE _CURRENCY_29SEP2021;


Select * From sys.all_objects where name like '%BK%' and type ='U'
DROP TABLE TRAN_SUMMARY_DETAIL_TEMP_BK00;
DROP TABLE SEC_GROUP_PERMISSION_BK00;
DROP TABLE SEC_APP_PERMISSION_MAP_BKP;
DROP TABLE SEC_PERMISSION_BKP;
DROP TABLE META_COUNTER_BKP;
DROP TABLE SCHEMA_METADATA_BKP;
DROP TABLE SCHEMA_METADATA_TABLE_BKP;
DROP TABLE BIZ_PROCESS_BKP;
DROP TABLE BIZ_PROCESS_STATES_BKP;


Select * From sys.all_objects where name like '%Old%' and type ='U'
DROP TABLE Biz_process_Old;
DROP TABLE Biz_process_State_old;


Select * From sys.all_objects where name like '%TEST%' and type ='U'
DROP TABLE biz_process_STATES_TEST;
DROP TABLE biz_process_TEST;


Select * From sys.all_objects where name like '%TMP00%' and type ='U'
DROP TABLE TRAN_SUMMARY_TEMP_TMP00;
DROP TABLE TRANSACTION_LOG_DETAIL_TMP00;
DROP TABLE TRANSACTION_LOG_TMP00;
DROP TABLE TRANSACTION_SUMMARY_TMP00;



Select * From sys.tables where name LIKE '%[0-9]%'




