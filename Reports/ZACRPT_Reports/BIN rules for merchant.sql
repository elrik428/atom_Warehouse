-- 1. Export to csv
select (CASE destport
WHEN 'NET_ABC'      THEN 'Piraeus Bank'
WHEN 'NET_CITI'     THEN 'CitiBank'
WHEN 'NET_CMBN'     THEN 'Commercial Bank'
WHEN 'NET_AGROTIKI' THEN 'Agrotiki Bank'
WHEN 'NET_NTBN'     THEN 'National Bank of Greece'
WHEN 'NET_BICALPHA'    THEN 'Alpha Bank'
WHEN 'NET_CLBICEBNK'     THEN 'Eurobank'
WHEN 'NET_GENIKI'   THEN 'Geniki Bank'
ELSE 'UNKNOWN BANK'
END), binlower, binupper from merchbins 

where tid in (select tid from merchants where mid = '000000001100009') 
group by destport, binlower, binupper
order by destport


-- 2. Import csv and run sql
select a.column1 as 'Routing Bank', c.BANK as'Issuing Bank', a.column2 as 'BinLower', a.column3 as 'BinUpper'
,b.Product from dbo.[bins_KOTSO_LN] a
join abc096.Products b on a.column2 = b.BIN
join abc096.Banks c on b.BANKID = c.ID
order  by a.column1