


::: RESET ORACLE USER LOGIN PASSWORD :::
------------------------------------------------------------------------------------------------

-- To unlock the user (if expired password)
alter user aspire_bbk account unlock;
alter user aspire_bbk identified by aspire;


-- Then 
alter profile DEFAULT limit PASSWORD_REUSE_TIME unlimited;

alter profile DEFAULT limit PASSWORD_LIFE_TIME  unlimited;



SELECT USERNAME, ACCOUNT_STATUS, EXPIRY_DATE FROM DBA_USERS where lower(username)='aspire_bbk';

