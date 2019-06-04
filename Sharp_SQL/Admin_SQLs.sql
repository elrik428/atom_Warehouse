
-- Schema Size
SELECT sum(bytes) / 1024 / 1024 / 1024 as "Size in GB"
  FROM dba_segments
 WHERE owner = UPPER('EUTMS2');

-- Count of tables
SELECT COUNT(*) FROM USER_TABLES

-- LIst with tables names
SELECT table_name FROM user_tables
order by table_name
