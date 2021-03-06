-- Update PPC
UPDATE    [ZacReporting].[dbo].[TRANSLOG_TRANSACT] 
SET MID=LEFT(MID,12)+'PPC'
    where left(mid,8)='00000017' and right(mid,4)<>'0000'
    and dtid not in 
    (
    '79300010',
    '07778608',
    '70006135',
    '79300011',
    '07778614',
    '70006136',
    '79300012',
    '07778609',
    '70006137'
    )

UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT]  
SET MID=
    (select left(mid,12)+ right(left([CASHIERINFO],4),3)
    from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]
    where [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly].tcode=dbo.TRANSLOG_TRANSACT.tcode and right(left([CASHIERINFO],4),3) in ('PPC','SRS'))
    where dbo.TRANSLOG_TRANSACT.mid in ('000000170000000','000000170010000','000000170020000')
    and dbo.TRANSLOG_TRANSACT.destcomid not in ('NET_EBLY1','NET_NTBNLTY','NET_PBGLTY')


UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT]  
SET MID=
    (select left(mid,12)+ 'SRS'
    from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]
    where [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly].tcode=dbo.TRANSLOG_TRANSACT.tcode)
    where dbo.TRANSLOG_TRANSACT.mid in ('000000170000000','000000170010000','000000170020000')
    and dbo.TRANSLOG_TRANSACT.destcomid  in ('NET_EBLY1','NET_NTBNLTY','NET_PBGLTY')