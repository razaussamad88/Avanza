


sqlplus / as sysdba

show parameter target

ALTER SYSTEM SET pga_aggregate_target = 249M SCOPE=SPFILE;

ALTER SYSTEM SET sga_target = 749M SCOPE=SPFILE;

ALTER SYSTEM SET memory_max_target = 1G SCOPE=SPFILE;

shutdown immediate

startup