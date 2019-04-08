SELECT [ALLOWED_ACTIONS]
  FROM [ZACRPT].[dbo].[MERCHANTS]
  where uploadhostid = '302' and substring(tid,1,2) = '73'
  group by [ALLOWED_ACTIONS]


111101333100

111101333300

110101333100
011101333100
111101331100
111101333101
111100331100

SELECT substring([ALLOWED_ACTIONS],1,9) + '3' + substring([ALLOWED_ACTIONS],11,2),*
FROM [ZACRPT].[dbo].[MERCHANTS]
where uploadhostid = '302' and substring(tid,1,2) = '73' and [ALLOWED_ACTIONS]= '111101333101'


update [ZACRPT].[dbo].[MERCHANTS]
set [ALLOWED_ACTIONS]= substring([ALLOWED_ACTIONS],1,9) + '3' + substring([ALLOWED_ACTIONS],11,2)
where uploadhostid = '302' and substring(tid,1,2) = '73' and [ALLOWED_ACTIONS]= '111101333100'
