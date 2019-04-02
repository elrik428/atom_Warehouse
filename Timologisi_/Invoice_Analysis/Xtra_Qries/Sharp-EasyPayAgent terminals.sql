SELECT distinct 
TER_TID as "Sharp TID",
tsa_serial as "S/N",
TER_MER_NAME as "ΔΙΑΚΡ. ΤΙΤΛΟΣ",
TER_MER_NAME2 as "ΔΙΕΥΘΥΝΣΗ",
TER_MER_ADDRESS as "ΠΟΛΗ",
TER_MER_CITY as "ΤΗΛΕΦΩΝΟ",
TER_MID as "Merchant ID",
TER_TID as "Terminal ID",
substr(tsa_serial,6,2) as "Terminal_Type",
TSA_VERSION as "Application_Version",
TER_MCC_TYPE as "MCC",
EPA_FINANCIAL as "Payment Availability",
EPA_EPA_ENABLED as "Bill Payment Availability"
FROM (TERMINAL join TERM_STAT_ACT on TERM_STAT_ACT.tsa_terminal=TERMINAL.ter_tid) join TERM_EASYPAYSERVICES on TERM_EASYPAYSERVICES.EPA_TERMINAL=TERMINAL.ter_tid
where ter_tid<>'01910360' and ter_tid in (
SELECT EPA_TERMINAL
FROM TERM_EASYPAYSERVICES 
where EPA_EPA_ENABLED=1)
order by ter_tid
;