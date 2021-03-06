-- TERMINAL
-- CHECK SQL
select ter_tid, ter_ecr, ter_ecr_channel from terminal
where ter_tid in('01104530','01104535','01104536','01104538','01104539','03822620','01822632');


-- TERM_TCP
-- CHECK SQL
select * from term_tcp
where tcp_terminal in ('01104530','01104535','01104536','01104538','01104539','03822620','01822632')
and tcp_port1 = 5000;

-- INSERT RECORDS
insert into term_tcp (TCP_TERMINAL, TCP_IDX,TCP_HOST1, TCP_PORT1, TCP_HOST2, TCP_PORT2,TCP_TIMEOUT,TCP_SHOW)
select '01104535', TCP_IDX,TCP_HOST1, TCP_PORT1, TCP_HOST2, TCP_PORT2,TCP_TIMEOUT,TCP_SHOW  from term_tcp
where tcp_terminal = '01361234' and tcp_port1 = 5000;


-- TERN_SERIAL
-- CHECK SQL
select TSE_TERMINAL,TSE_IDX,TSE_PORT,TSE_BAUDRATE,TSE_BITNR,TSE_PARITY,TSE_STOPBIT,TSE_SHOW from term_serial
WHERE TSE_TERMINAL IN 
('01104530','01104535','01104536','01104538','01104539','03822620','01822632')

-- INSERT RECORDS
INSERT INTO term_serial(TSE_TERMINAL,TSE_IDX,TSE_PORT,TSE_BAUDRATE,TSE_BITNR,TSE_PARITY,TSE_STOPBIT,TSE_SHOW)
select '01104530',TSE_IDX,TSE_PORT,TSE_BAUDRATE,TSE_BITNR,TSE_PARITY,TSE_STOPBIT,TSE_SHOW from term_serial
where TSE_TERMINAL= '01361234' AND TSE_BAUDRATE ='19200';

INSERT INTO term_serial(TSE_TERMINAL,TSE_IDX,TSE_PORT,TSE_BAUDRATE,TSE_BITNR,TSE_PARITY,TSE_STOPBIT,TSE_SHOW)
select '01104530',TSE_IDX,TSE_PORT,TSE_BAUDRATE,TSE_BITNR,TSE_PARITY,TSE_STOPBIT,TSE_SHOW from term_serial
where TSE_TERMINAL= '01361234' AND TSE_BAUDRATE ='115200';