
---   DOWNLOADED
select distinct vc30.relation.termid as TID, a.[value], b.[value] , c.[value] , d.[value], e.[value] ,
vc30.TERMLOG.EVDATE as DOWNLOADED,
vc30.TERMLOG.famnm,vc30.TERMLOG.message,vc30.TERMLOG.status,vc30.TERMLOG.appnm, vc30.TERMLOG.duration
from (vc30.relation left join vc30.TERMLOG on
      vc30.relation.TERMID=vc30.TERMLOG.TERMID
      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC ='MERCHID'
      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC='RECEIPT_LINE_1'
      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC='RECEIPT_LINE_2'
      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC='RECEIPT_LINE_3'
      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC='RECEIPT_LINE_4'
      )
where
      (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
      vc30.TERMLOG.appnm like '%PIRA0203P%' and
      vc30.TERMLOG.message = 'Download Successful' and
      vc30.TERMLOG.EVDATE > '2018-05-04 00:00:00.000' and
      vc30.TERMLOG.TERMID in
   ('00011977','00096771','00204191','00226275','00241252','01000553','01000640','01001126','01002430','01003340','01006200','01008180','01009160','01010180','01010610','01011290','01012060','01013060','01013130','01013150','01013324','01014520','01014730','01015790','01016340','01016580','01027241','01027774','01028260','01031775','01031990','01032032','01033601','01033830','01034930','01035031','01035932','01038620','01041250','01042480','01043630','01044570','01046560','01046600','01046810','01047140','01049050','01049880','01051600','01052940','01054671','01055830','01056260','01056650','01059660','01061370','01061480','01062073','01062350','01062371','01063530','01063904','01063950','01065450','01066490','01067120','01070042','01070810','01071820','01073490','01075365','01076100','01078921','01078934','01078953','01079041','01079062','01079550','01079951','01080390','01083551','01085100','01085190','01088641','01088910','01089683','01090010','01090822','01092301','01093040','01093100','01093151','01093500','01096650','01097640','01107410','01107901','01115021','01116790','01117084','01123560','01132878','01139811','01141262','01141343','01153003','01160800','01163710','01165112','01173481','01174281','01175913','01182363','01183470','01184222','01184900','01187421','01187471','01187590','01187720','01191010','01192841','01193821','01197360','01197706','01203342','01203541','01212402','01212702','01215160','01215301','01215740','01216011','01216041','01216481','01216860','01219030','01220080','01223090','01224011','01226001','01227440','01233920','01233975','01238002','01240581','01240612','01242732','01243280','01243720','01243981','01244540','01249861','01250000','01253991','01254226','01255012','01255912','01256151','01256220','01257070','01257542','01257551','01257561','01259741','01266321','01266540','01268300','01268640','01271270','01271970','01272122','01275644','01278171','01278940','01280732','01280910','01282203','01282861','01287041','01287152','01288542','01289821','01289953','01291440','01291791','01298230','01300842','01300950','01301160','01303670','01304042','01304210','01308601','01309140','01309621','01310361','01311491','01314760','01315891','01317250','01318880','01320542','01321602','01323640','01324861','01325821','01326420','01327331','01327501','01328450','01331321','01332421','01335660','01337681','01339110','01339640','01342640','01343330','01344480','01346530','01348460','01357780','01359442','01360112','01360282','01361360','01362190','01363611','01363671','01363881','01365201','01366090','01366440','01367130','01367491','01370390','01371160','01371440','01371860','01371901','01373041','01373511','01373640','01375341','01376861','01377152','01378651','01379980','01380081','01382843','01383790','01385560','01388550','01389760','01395001','01395021','01397271','01400862','01402551','01403890','01404221','01405330','01405550','01406191','01407831','01409671','01410171','01412120','01413310','01415150','01419910','01421680','01424350','01425091','01425170','01428240','01428670','01431130','01436280','01438650','01439300','01440180','01440350','01441310','01442830','01443350','01449590','01451680','01452912','01454870','01455790','01458054','01463141','01463152','01465291','01465840','01466791','01467310','01471000','01473561','01475280','01475852','01476711','01477070','01478350','01479880','01480581','01480583','01482570','01482852','01483882','01487210','01487850','01488281','01488440','01489161','01489311','01490080','01490271','01491370','01491500','01491541','01492781','01493590','01494200','01494960','01498621','01500060','01500221','01501780','01501990','01502940','01503350','01503400','01504896','01506441','01506981','01507060','01509940','01510142','01510460','01510921','01511701','01512160','01512620','01513150','01513660','01515460','01516120','01516210','01516830','01525770','01526100','01526190','01526460','01527890','01529280','01530990','01531610','01533710','01534220','01535380','01536670','01536880','01537150','01539740','01539830','01541820','01544601','01545330','01547710','01548480','01549210','01549653','01550650','01551340','01553400','01554561','01555581','01559430','01560041','01560862','01561721','01563250','01564321','01564630','01569350','01569450','01571060','01572990','01573320','01573631','01573750','01574710','01575860','01576580','01581420','01586680','01588230','01590901','01594170','01594791','01597090','01597170','01598470','01601110','01606750','01609040','01609670','01610020','01610200','01612651','01614930','01616290','01616830','01618710','01618721','01620000','01620870','01621571','01621821','01622080','01624790','01625220','01626020','01626630','01627350','01627660','01627770','01628770','01630190','01630590','01632940','01639422','01642410','01642840','01643892','01645061','01645570','01645630','01646460','01646974','01649280','01649710','01653450','01653700','01654260','01654320','01654410','01654860','01657700','01657910','01658551','01660240','01662120','01664750','01664900','01666215','01666780','01667020','01668670','01669230','01676130','01680891','01681790','01684400','01684822','01684970','01684981','01686660','01687810','01689420','01692101','01692250','01692911','01695541','01696001','01699430','01700070','01701961','01703000','01706780','01706951','01709042','01709520','01709530','01713730','01714720','01715780','01715980','01716185','01716590','01717750','01719563','01719900','01720780','01721320','01722270','01722910','01723170','01727870','01728630','01730910','01732530','01732830','01737081','01737180','01737390','01737810','01739330','01741870','01741920','01742011','01744431','01746060','01746350','01747560','01748252','01748260','01749811','01750940','01754490','01755020','01755530','01756120','01756300','01757410','01757700','01758681','01759810','01760070','01760311','01760390','01761270','01761900','01761940','01763920','01764470','01765230','01765412','01765420','01767790','01769080','01769146','01769701','01769760','01770370','01771930','01772110','01773580','01774452','01774680','01774800','01775810','01775870','01776150','01777320','01777811','01778820','01780700','01781070','01781211','01781241','01782743','01782802','01782912','01785020','01785090','01786200','01788910','01790320','01790611','01790851','01791080','01792390','01793851','01795331','01795610','01796110','01798430','01799181','01799331','01800150','01800861','01805011','01807240','01807570','01809060','01809150','01811531','01813230','01813350','01816290','01816741','01818681','01819160','01824470','01825330','01825880','01826300','01826570','01827170','01827710','01829310','01831140','01831150','01833730','01835170','01836180','01836711','01839400','01839410','01839931','01839940','01840350','01841020','01841730','01842910','01843070','01843680','01843770','01845250','01846280','01846451','01847920','01848190','01849160','01850210','01850690','01851050','01851850','01852250','01852330','01854424','01854710','01855991','01856230','01857480','01858030','01858190','01858592','01860370','01861061','01861070','01861160','01861781','01864110','01864811','01867891','01868381','01868711','01869540','01870350','01870510','01871240','01872360','01872380','01874260','01874500','01874770','01875220','01878342','01881410','01882891','01884031','01884281','01884640','01884840','01884951','01886140','01886911','01892562','01892832','01894832','01895431','01896491','01896620','01896700','01896900','01897511','01898350','01898370','01898890','01900251','01900770','01901230','01902150','01902250','01902590','01902961','01903240','01903592','01903770','01904191','01904370','01904440','01905900','01906021','01907691','01908872','01909760','01910630','01910831','01910940','01911190','01913341','01913371','01914432','01914590','01916112','01916513','01916652','01916771','01916861','01917221','01917672','01918842','01920152','01932570','01941350','01946370','01957030','01957042','01970061','01977080','01977670','01978770','01979630','01980240','01982920','01986690','01987450','01987840','01989460','01990360','01991270','01993300','01994020','01994660','01994780','01994960','01995380','01997450','01997730','01998780','02273125','19811038','19811132','19811151','19811325','19811498','19811664','19811688','19811759','19811794','19811799','19811804')



   ---  NOT DOWNLOADED
   select distinct vc30.relation.termid as TID
   from  vc30.relation
   where vc30.relation.clusterid = 'EPOS_DUTYFREE'
   and vc30.relation.termid not in ( select vc30.TERMLOG.TERMID from vc30.TERMLOG where
         (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         vc30.TERMLOG.appnm like '%EPOS0213P%' and
         vc30.TERMLOG.message = 'Download Successful' and
         vc30.TERMLOG.EVDATE > '2018-09-24 16:00:00.000')


-- For Donna
         select distinct q.PARTID as TID
            from  (SELECT [PARTID]  FROM [vc30].[vc30].[PARAMETER]
         		  where parnameloc='services_enabled' and [value]='TRUE' and partid not like '73%'
         		   and partid not in ('520CHAL','520CHALREC','520KOTSO','675CHALREC','KOTSMCCT','KOTSO675','KOTSTST','8508TEST','TESTDONN','520EPPCAR','520EPPHOT','520EPPINST','520EPPRES','520EPPSAL','520EPPSUP','520EPPTRA','520GSERV','675EPPCAR','675EPPHOT','675EPPINST','675EPPRES','675EPPSAL','675EPPSUP','675EPPTRA','COMBOSERV','TESTDON2')) q
            where q.PARTID not in
            (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         	where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         	 vc30.TERMLOG.message = 'Download Successful' and
         	 vc30.TERMLOG.EVDATE > '2018-11-10 11:00:00.000')


--For Antonis
select distinct q.PARTID as TID
from  (select PARTID from vc30.PARAMETER where
		PARNAMELOC = 'SERVICES_ENABLED' and [value] = 'TRUE' and substring(partid,1,3) not in ('520','675','820','690','COM','KOT','TES') and substring(partid,1,5) not in ('8508T')) q
where q.PARTID not in  (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         				where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         				vc30.TERMLOG.message = 'Download Successful' and
         				vc30.TERMLOG.EVDATE > '2018-12-11 11:00:00.000')
