--ZAC_PRT

--all
select count(*) as No_of_POS_EMV,count(distinct merchaddress) as MERCHANT_LOCATIONS,count(distinct MERCHCITY) as MERCHANT_AFM from dbo.MERCHANTS where
uploadhostid in('2','202','302')
and merchtitle not like '%WINBANK%'
and merchtitle not like '%TEST %'
and merchtitle <>'000000000219854-00048782'
and merchtitle<>'TEST'
--and CREATED_DATE<'2015-02-28 00:01:47.147'
;
--CTLS only
select count(*) as No_of_POS_EMV,count(distinct merchaddress) as MERCHANT_LOCATIONS,count(distinct MERCHCITY) as MERCHANT_AFM from dbo.MERCHANTS where
uploadhostid in('2','202','302')
and merchtitle not like '%WINBANK%'
and merchtitle not like '%TEST %'
and merchtitle <>'000000000219854-00048782'
and merchtitle<>'TEST'
and (substring(allowed_actions,7,1) ='3' or substring(allowed_actions,8,1) ='3' )
--and CREATED_DATE<'2015-02-28 00:01:47.147'
