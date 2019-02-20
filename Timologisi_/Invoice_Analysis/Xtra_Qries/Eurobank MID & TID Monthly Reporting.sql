select mdcmid,desttid,
CTLs=case
when (substring(allowed_actions,7,1) ='3' or substring(allowed_actions,8,1) ='3' ) then 'CTLs-Y'
else
'CTLs-N'
End,
merchtitle,
--MERCHADDRESS,
merchaddr=case
when substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,1) in ('1','2','3','4','5','6','7','8','9','0')
     and right(substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,5),1) in ('1','2','3','4','5','6','7','8','9','0')
then
left(MERCHADDRESS,(charindex (',',MERCHADDRESS))-1)
else
merchaddress
end,
merchCity=
case
when substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,1) in ('1','2','3','4','5','6','7','8','9','0')
     and right(substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,5),1) in ('1','2','3','4','5','6','7','8','9','0')
then
right(MERCHADDRESS,len(merchaddress)-(charindex (' ',MERCHADDRESS,(charindex (', ',MERCHADDRESS))+2)))
else
' '
end,
merchzip=
case
when substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,1) in ('1','2','3','4','5','6','7','8','9','0')
     and right(substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,5),1) in ('1','2','3','4','5','6','7','8','9','0')
then
substring(MERCHADDRESS,(charindex (',',MERCHADDRESS))+2,5)
else
' '
end,merchtype
--mid,tid
from dbo.MERCHANTS where
uploadhostid in ('4','205')
and merchtitle not like '%WINBANK%'
and tid<>' '
and tid is not null
and merchtitle not like '%TEST %'
and merchtitle <>'000000000219854-00048782'
and merchtitle<>'TEST'
order by  merchtitle, MERCHADDRESS,mdcmid,desttid,tid,mid
;

