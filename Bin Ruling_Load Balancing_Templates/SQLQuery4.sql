select * from merchbins where tid in (select tid from merchants where mid = '000000170000000') --and binlower = '516297' 
and tid = '73005244'
--and tid = '73005244' 
--and destport = 'NET_NTBN'
 --and binlower not in ('516297','410788','422164','459346','491791','527801','527890','535142','552053','589242','442317')
order by binlower


--update merchbins 
--set instmax = 1
--where tid in (select tid from merchants where mid = '000000170000000')
---- and tid = '73005244'
--and binlower = '516297'


select * from dbo.merchbins
where tid = '1111    ' and binlower = '930050'

--update  merchbins 
--set destport = 'NET_CLBICEBNK'
--where tid in (select tid from merchants where mid = '000000170000000') 
----and tid = '73005244' 
--and destport = 'NET_NTBN' and binlower not in ('516297','410788','422164','459346','491791','527801','527890','535142','552053','589242','442317')




NET_CLBICEBNK