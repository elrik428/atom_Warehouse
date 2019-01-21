
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
case TSA_COMM_MODE  when  '1|X0;I0' THEN 'Ethernet'  when  '2|X0;I0' THEN 'Dialup'  when  '1|G0X0;X0' THEN 'GPRS'  else
   (case substr(TERMINAL.TER_DL_CHANNEL,1,2) when 'X0' then 'Ethernet' when 'I0' then 'Dialup' when 'G0' then 'GPRS' else 'N/A' end)
  end as Connection_Mode,
'N/A' as TYPOS_KARTAS_SIM,
'N/A' as AYTOMATI_APOSTOLI_PAKETOY,
'YES' as CONTACTLESS,
'N/A' as FOROKARTA,
TERMINAL.TER_START_PARAM_DL as LAST_PARAMETER_CALL,
case TERMINAL.TER_ECR when 1 then 'YES' when 0 then 'NO' else 'N/A' end as SYNDESI_TAMEIAKIS,
case substr(TERMINAL.ter_funcs,3,1) when '1' then 'YES' when '0' then 'NO' else 'N/A' end as TIP,
'NO' as SYNDESI_PINPAD,
TERM_STAT_ACT.TSA_VERSION as EKDOSI_EFARMOGIS,
'ICT_220' as MONTELO_TERMATIKOU,
'Ingenico' as KATASKEVASTIS,
case (TERM_LOYALTY_PBG_PAR.PBG_LOYALTY_PBG) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end as LOYALTY,
TERMINAL.TER_CRE_AT as BIRTH_DATE
from (((((TERMINAL left join TERM_CARD_TYPE_RANGE ON TERMINAL.TER_TID=TERM_CARD_TYPE_RANGE.TCR_TERMINAL AND TERM_CARD_TYPE_RANGE.TCR_CARD_TYPE=1)
left join TERM_LOYALTY_PBG_PAR ON TERM_LOYALTY_PBG_PAR.PBG_TERMINAL=TERMINAL.TER_TID)
left join TERM_CARD_TYPE t1 ON t1.TCT_TERMINAL=TERMINAL.TER_TID AND t1.TCT_CARD_TYPE=29)
left join TERM_CARD_TYPE t2 ON t2.TCT_TERMINAL=TERMINAL.TER_TID AND t2.TCT_CARD_TYPE=30)
left join TERM_STAT_ACT ON TERM_STAT_ACT.TSA_TERMINAL=TERMINAL.TER_TID)
where TERMINAL.TER_CLUSTER IN (
303,--PIRA ICT SALES
304,--PIRA ICT INSTALMENTS
305,--PIRA ICT CAR
306,--PIRA ICT HOTEL
307,--PIRA ICT RESTAURANT
308,--PIRA ICT SUPER MARKET
309--PIRA ICT TRAVEL
) and TERMINAL.TER_STATUS=1
order by TERMINAL.TER_TID
