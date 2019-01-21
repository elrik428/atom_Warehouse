select  distinct TERMINAL.TER_TID as TID,
TERMINAL.TER_MID as MID,
TERMINAL.TER_MER_NAME as DIAKRITIKOS_TITLOS,
TERMINAL.TER_MER_NAME2  as DIEFTHINSI,
TERMINAL.TER_MER_ADDRESS as POLI,
TERMINAL.TER_MER_CITY as TILEFONO,
case substr(TERMINAL.ter_funcs,6,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as DOSEIS,
'N/A' AS PLITHOS_DOSEON,
case TERMINAL.TER_MESSGL when 'GREN' then 'Greek' else 'English' end as GLOSSA,
case substr(TERM_CARD_TYPE_RANGE.TCR_OPTIONS,8,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as PLIKTROLOGISI,
case substr(TERMINAL.ter_flags,7,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as CVV,
case substr(TERMINAL.ter_funcs,7,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as PROEGKRISI,
case substr(TERMINAL.ter_funcs,8,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as COMPLETION,
case substr(TERMINAL.ter_flags,9,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as DCC,
case t1.TCT_CARD_TYPE when 29 then 'YES' else 'NO' end as CUP,
case t2.TCT_CARD_TYPE when 30 then 'YES' else 'NO' end as KARTA_SYMVOLAIAKIS,
case substr(TERMINAL.TER_DL_CHANNEL,1,2) when 'X0' then 'Ethernet' when 'I0' then 'Dialup' when 'G0' then 'GPRS' else 'N/A' end as SINDESI,
'N/A' as TYPOS_KARTAS_SIM,
'N/A' as AYTOMATI_APOSTOLI_PAKETOY,
'YES' as CONTACTLESS,
'N/A' as FOROKARTA,
TERMINAL.TER_START_PARAM_DL as LAST_PARAMETER_CALL,
case TERMINAL.TER_ECR when 1 then 'YES' when 0 then 'NO' else 'N/A' end as SYNDESI_TAMEIAKIS,
case substr(TERMINAL.ter_flags,11,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as TIP,
'NO' as SYNDESI_PINPAD,
TERM_STAT_ACT.TSA_VERSION as EKDOSI_EFARMOGIS,
CASE substr(tcl_name,1,8) when 'PIRA IWL' then 'IWL_220' when 'PIRA ICT' then 'ICT_220' else 'N/A' end as  MONTELO_TERMATIKOU,
'Ingenico' as KATASKEVASTIS,
case (TERM_LOYALTY_PBG_PAR.PBG_LOYALTY_PBG) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end as LOYALTY,
TERMINAL.TER_CRE_AT as BIRTH_DATE,
TERMINAL.TER_CLUSTER as CLUSTER_,
TERM_CLUSTER.TCL_NAME AS CLUSTER_DESCR_,
TERMINAL.TER_MCC_TYPE AS MCC,
case substr(TERMINAL.TER_FUNCS,2,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as REFUND_STATUS,
case TERM_EASYPAYSERVICES.EPA_EPA_ENABLED when 1 then 'YES' when 0 then 'NO' end as Easypaypoint
from (((((((TERMINAL left join TERM_CARD_TYPE_RANGE ON TERMINAL.TER_TID=TERM_CARD_TYPE_RANGE.TCR_TERMINAL AND TERM_CARD_TYPE_RANGE.TCR_CARD_TYPE=1)
left join TERM_LOYALTY_PBG_PAR ON TERM_LOYALTY_PBG_PAR.PBG_TERMINAL=TERMINAL.TER_TID)
left join TERM_CARD_TYPE t1 ON t1.TCT_TERMINAL=TERMINAL.TER_TID AND t1.TCT_CARD_TYPE=29)
left join TERM_CARD_TYPE t2 ON t2.TCT_TERMINAL=TERMINAL.TER_TID AND t2.TCT_CARD_TYPE=30)
left join TERM_STAT_ACT ON TERM_STAT_ACT.TSA_TERMINAL=TERMINAL.TER_TID)
left join TERM_EASYPAYSERVICES ON TERM_EASYPAYSERVICES.EPA_TERMINAL = TERMINAL.TER_TID)
left join TERM_CLUSTER ON TERM_CLUSTER.TCL_ID = TERMINAL.TER_CLUSTER)
where TERMINAL.TER_CLUSTER IN (303, 304,305,306,307,308,309,310,311,312,313,314,315,316) and TERMINAL.TER_STATUS=1
order by TERMINAL.TER_TID
